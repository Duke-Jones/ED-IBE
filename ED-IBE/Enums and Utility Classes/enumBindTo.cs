using System;

   // <summary>
   //  helperclass for binding comboboxes to enumerations
   // </summary>
   // <remarks></remarks>
   public class enumBindTo{

      private Int32     m_EnumValue;        // enum value
      private string    m_EnumString;      // text showing in combobox

      public Int32 EnumValue{
         get{
            return m_EnumValue;
         }
         set{
            m_EnumValue = value;
         }
      }

      public string EnumString
      {
         get{
            return m_EnumString;
         }
         set{
            m_EnumString = value;
         }
      }

   } 
