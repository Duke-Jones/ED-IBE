using System;
using System.Collections.Generic;
using System.Linq;
using IBE.Enums_and_Utility_Classes;
using System.Diagnostics;

namespace IBE.IBECompanion
{
    class CompanionConverter
    {
        private List<String> ignoreStuff = new List<String>() {"bobble", "decal", "paintjob"};

        /// <summary>
        /// converts companion data to EDDN usable data.
        /// Information and process flow gathered from "ED-Market Connector 2.1.0.0"
        /// Many thanks to Marginal for this work - https://github.com/Marginal/EDMarketConnector
        /// </summary>
        /// <param name="outfittingItem"></param>
        /// <param name="entitled"></param>
        /// <returns></returns>
        public EDDN.OutfittingObject GetOutfittingFromCompanion(Newtonsoft.Json.Linq.JToken outfittingItem, Boolean entitled = false)
        {
            EDDN.OutfittingObject outfitting = null;  
            List<String> nameParts;
            String nameFull;
            List<String> category;
            String skuString = null;
            Tuple<string, string> dataTuple;

            try
            {
               
                if(outfittingItem != null)
                { 
                    if(outfittingItem.SelectToken("name", false) == null)
                        throw new NotSupportedException(String.Format("{0}: Missing name", outfittingItem.SelectToken("id")));

                    nameFull  = outfittingItem.SelectToken("name", false).ToString().ToLower();
                    nameParts = nameFull.Split(new char[] {'_'}).ToList();

                    skuString = (outfittingItem.SelectToken("sku", false) ?? "").ToString();

                    if(nameFull.Contains("planet"))
                        Debug.Print("!");

                    // Armour - e.g. Federation_Dropship_Armour_Grade2
                    if(nameParts[nameParts.Count-2] == "armour")
                    {
                        //Armour is ship-specific, and ship names can have underscores
                        outfitting = new EDDN.OutfittingObject();

                        outfitting.Category = EDDN.OutfittingObject.Cat_Standard;
                        outfitting.Name     = Program.Data.GetMapping("armour", nameFull.Substring(nameFull.LastIndexOf("_") + 1));
                        outfitting.Ship     = Program.Data.GetMapping("ships",  nameFull.Substring(0, nameFull.IndexOf("armour") - 1));
                        outfitting.Class    = "1";
                        outfitting.Rating   = "I";
                    }
                    else if(ignoreStuff.Contains(nameParts[0]))
                    {
                        // Skip uninteresting stuff    
                    }
                    else if((!entitled) && (!String.IsNullOrEmpty(skuString)) && ((skuString != "ELITE_HORIZONS_V_PLANETARY_LANDINGS") || (nameParts[1] == "planetapproachsuite")))
                    {
                        // Shouldn't be listing player-specific paid stuff in outfitting, other than Horizons
                    }
                    else if(nameParts[0] == "hpt")
                    {
                        if(Program.Data.GetMapping("weapon", nameParts[1], false) != null)
                        {
                            // Hardpoints - e.g. Hpt_Slugshot_Fixed_Medium
                            outfitting          = new EDDN.OutfittingObject();
                            outfitting.Category = EDDN.OutfittingObject.Cat_Hardpoint;

                            if(Program.Data.GetMapping("weaponmount", nameParts[2], false) == null)
                                throw new NotSupportedException(String.Format("{0}: Unknown weapon mount {1}", outfittingItem.SelectToken("id"), nameParts[2]));
                            if(Program.Data.GetMapping("weaponclass", nameParts[3], false) == null)
                                throw new NotSupportedException(String.Format("{0}: Unknown weapon class {1}", outfittingItem.SelectToken("id"), nameParts[3]));

                            if(nameParts.Count() > 4)
                            {
                                if(Program.Data.GetMapping("oldvariant", nameParts[4], false) != null)
                                {
                                    // Old variants e.g. Hpt_PulseLaserBurst_Turret_Large_OC
                                    outfitting.Name     = String.Format("{0} {1}", Program.Data.GetMapping("weapon", nameParts[1]), Program.Data.GetMapping("oldvariant", nameParts[4]));
                                    outfitting.Rating   = "?";
                                }
                                else
                                {
                                    // PP faction-specific weapons e.g. Hpt_Slugshot_Fixed_Large_Range
                                    outfitting.Name     = Program.Data.GetMapping("weapon", nameParts[1], nameParts[4]);
                                    outfitting.Rating   = Program.Data.GetMapping("weaponrating", String.Join("_", nameParts.GetRange(0,4)), false) ?? "?";
                                }
                            }
                            else
                            {
                                // # no obvious rule - needs lookup table
                                outfitting.Name     = Program.Data.GetMapping("weapon", nameParts[1]);
                                outfitting.Rating   = Program.Data.GetMapping("weaponrating", nameFull, false) ?? "?";
                            }

                            outfitting.Mount = Program.Data.GetMapping("weaponmount", nameParts[2]);
                            outfitting.Class = Program.Data.GetMapping("weaponclass", nameParts[3]);

                            if(Program.Data.GetMapping("missiletype", nameParts[1], false) != null)
                            {
                                // e.g. Hpt_DumbfireMissileRack_Fixed_Small
                                outfitting.Guidance = Program.Data.GetMapping("missiletype", nameParts[1]);
                            }
                        }
                        else if(Program.Data.GetMapping("countermeasure", nameParts[1], false) != null)
                        {
                            // Countermeasures - e.g. Hpt_PlasmaPointDefence_Turret_Tiny
                            outfitting          = new EDDN.OutfittingObject();
                            outfitting.Category = EDDN.OutfittingObject.Cat_Utility;

                            if(nameParts.Count() > 4)
                                dataTuple = Program.Data.GetMappingT("countermeasure", nameParts[1], nameParts[4]);
                            else
                                dataTuple = Program.Data.GetMappingT("countermeasure", nameParts[1]);

                            outfitting.Name     = dataTuple.Item1;
                            outfitting.Rating   = dataTuple.Item2;
                            outfitting.Class    = Program.Data.GetMapping("weaponclass", nameParts[nameParts.Count-1]);
                        }
                        else if(Program.Data.GetMapping("utility", nameParts[1], false) != null)
                        {
                            // Utility - e.g. Hpt_CargoScanner_Size0_Class1
                            outfitting          = new EDDN.OutfittingObject();
                            outfitting.Category = EDDN.OutfittingObject.Cat_Utility;

                            if(nameParts.Count() > 4)
                                outfitting.Name = Program.Data.GetMapping("utility", nameParts[1], nameParts[4]);
                            else
                                outfitting.Name = Program.Data.GetMapping("utility", nameParts[1]);

                            if((!nameParts[2].StartsWith("size")) || (!nameParts[3].StartsWith("class")))
                                throw new NotSupportedException(String.Format("{0}: Unknown class/rating {1}/{2}", outfittingItem.SelectToken("id").ToString(), nameParts[2], nameParts[3]));

                            outfitting.Class    = nameParts[2].Substring(4);
                            outfitting.Rating   = Program.Data.GetMapping("rating", nameParts[3].Substring(5));
                        }
                        else
                        {
                            throw new NotSupportedException(String.Format("{0}: Unknown item {1}", outfittingItem.SelectToken("id").ToString(), nameParts[1]));
                        }
                    }
                    else if(nameParts[0] == "int")
                    {
                        if(nameParts[1] == "planetapproachsuite")
                        {
                            // Horizons Planetary Approach Suite
                            // only listed in outfitting if the user is *playing* Horizons
                            outfitting = new EDDN.OutfittingObject();

                            outfitting.Category     = EDDN.OutfittingObject.Cat_Standard;
                            outfitting.Name         = "Planetary Approach Suite";
                            outfitting.Class        = "1";
                            outfitting.Rating       = "I";
                            outfitting.Entitlement  = "horizons";
                        }
                        else if((nameParts.Count() > 2) && (Program.Data.GetMapping("internal_misc", nameParts[1], nameParts[2], false) != null))
                        {
                            // Miscellaneous Class 1 - e.g. Int_StellarBodyDiscoveryScanner_Advanced, Int_DockingComputer_Standard
                            // Reported category is not necessarily helpful. e.g. "Int_DockingComputer_Standard" has category "utility"
                            outfitting = new EDDN.OutfittingObject();

                            dataTuple = Program.Data.GetMappingT("internal_misc", nameParts[1], nameParts[2]);

                            outfitting.Category     = EDDN.OutfittingObject.Cat_Internal;
                            outfitting.Name         = dataTuple.Item1;
                            outfitting.Rating       = dataTuple.Item2;
                            outfitting.Class        = "1";
                        }
                        else
                        {
                            // Standard & Internal

                            if(nameParts[1] == "dronecontrol")
                            {
                                // e.g. Int_DroneControl_Collection_Size1_Class1
                                nameParts.RemoveAt(0);
                            }

                            if(Program.Data.GetMapping("standard", nameParts[1], false) != null)
                            {
                                // e.g. Int_Engine_Size2_Class1, Int_ShieldGenerator_Size8_Class5_Strong
                                outfitting = new EDDN.OutfittingObject();

                                outfitting.Category     = EDDN.OutfittingObject.Cat_Standard;
                                if(nameParts.Count() > 4)
                                    outfitting.Name = Program.Data.GetMapping("standard", nameParts[1], nameParts[4]);
                                else
                                    outfitting.Name = Program.Data.GetMapping("standard", nameParts[1]);
                            }
                            else if(Program.Data.GetMapping("internal", nameParts[1], false) != null)
                            {
                                // e.g. Int_CargoRack_Size8_Class1
                                outfitting = new EDDN.OutfittingObject();

                                outfitting.Category     = EDDN.OutfittingObject.Cat_Internal;
                                if(nameParts.Count() > 4)
                                    outfitting.Name = Program.Data.GetMapping("internal", nameParts[1], nameParts[4]);
                                else
                                    outfitting.Name = Program.Data.GetMapping("internal", nameParts[1]);
                            }
                            else
                            {
                                throw new NotSupportedException(String.Format("{0}: Unknown module {1}", outfittingItem.SelectToken("id"), nameParts[1]));
                            }

                            if((!nameParts[2].StartsWith("size")) || (!nameParts[3].StartsWith("class")))
                                throw new NotSupportedException(String.Format("{0}: Unknown class/rating {1}/{2}", outfittingItem.SelectToken("id"), nameParts[2], nameParts[3]));

                            outfitting.Class    = nameParts[2].Substring(4);
                            if(nameParts[1] == "buggybay")
                                outfitting.Rating   = Program.Data.GetMapping("ratingplanet", nameParts[3].Substring(5));
                            else
                                outfitting.Rating   = Program.Data.GetMapping("rating", nameParts[3].Substring(5));

                        }
                    }
                    else
                        throw new NotSupportedException(String.Format("{0}: Unknown prefix {1}", outfittingItem.SelectToken("id"), nameParts[0]));

                    if(outfitting != null)
                    {
                        //  Disposition of fitted modules
                        if((outfittingItem.SelectToken("on", false) != null) && (outfittingItem.SelectToken("priority", false) != null))
                        {
                            outfitting.Enabled   = outfittingItem.SelectToken("on").ToString();
                            outfitting.Priority  = outfittingItem.SelectToken("priority").ToString();
                        }

                        // Entitlements
                        if(!String.IsNullOrEmpty(skuString))
                        {
                            if(skuString.StartsWith("ELITE_SPECIFIC_V_POWER"))
                            {     
                                outfitting.Entitlement ="powerplay";
                            }
                            else if(skuString != "ELITE_HORIZONS_V_PLANETARY_LANDINGS")
                            {
                                throw new NotSupportedException(String.Format("{0}: Unknown sku {1}", outfittingItem.SelectToken("id"), skuString));
                            }

                        }

                        outfitting.Id = outfittingItem.SelectToken("id").ToString();

                        if((outfitting.Id == null) || (outfitting.Category == null) || (outfitting.Name == null) || (outfitting.Class == null) || (outfitting.Rating == null))
                            throw new NotSupportedException(String.Format("{0}: failed to set defaults", outfittingItem.SelectToken("id")));

                        if((outfitting.Category == EDDN.OutfittingObject.Cat_Hardpoint) && (outfitting.Mount == null))
                            throw new NotSupportedException(String.Format("{0}: failed to set hardpoint", outfittingItem.SelectToken("id")));
                    }
                }
                else
                    Debug.Print("!");

                return outfitting;

            }
            catch (NotSupportedException ex)
            {
                Program.MainLog.Log(String.Format("Converting error: {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new Exception("Error while converting companion data to outfitting object", ex);
            }

            return null;
        }
    }
}
