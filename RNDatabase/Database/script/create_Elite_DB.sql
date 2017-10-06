-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema elite_db
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `elite_db` ;

-- -----------------------------------------------------
-- Schema elite_db
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `elite_db` DEFAULT CHARACTER SET utf8 ;
USE `elite_db` ;

-- -----------------------------------------------------
-- Table `elite_db`.`tbAllegiance`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbAllegiance` (
  `id` INT NOT NULL,
  `allegiance` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbEconomy`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbEconomy` (
  `id` INT NOT NULL,
  `economy` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbGovernment`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbGovernment` (
  `id` INT NOT NULL,
  `government` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbState`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbState` (
  `id` INT NOT NULL,
  `state` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbSecurity`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbSecurity` (
  `id` INT NOT NULL,
  `security` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbPower`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbPower` (
  `id` INT NOT NULL,
  `power` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbPowerState`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbPowerState` (
  `id` INT NOT NULL,
  `powerstate` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbVisitType`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbVisitType` (
  `id` TINYINT NOT NULL,
  `VisitType` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbSystems`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbSystems` (
  `id` INT NOT NULL,
  `systemname` VARCHAR(80) NOT NULL,
  `x` DOUBLE NULL,
  `y` DOUBLE NULL,
  `z` DOUBLE NULL,
  `faction` VARCHAR(80) NULL,
  `population` MEDIUMTEXT NULL,
  `government_id` INT NULL,
  `allegiance_id` INT NULL,
  `state_id` INT NULL,
  `security_id` INT NULL,
  `primary_economy_id` INT NULL,
  `needs_permit` TINYINT(1) NULL,
  `updated_at` DATETIME NOT NULL,
  `is_changed` TINYINT(1) NULL DEFAULT 0,
  `visited` TINYINT NOT NULL DEFAULT 0,
  `Power_id` INT NULL,
  `PowerState_id` INT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_tbSystems_tbAllegiance1_idx` (`allegiance_id` ASC),
  INDEX `fk_tbSystems_tbEconomy1_idx` (`primary_economy_id` ASC),
  INDEX `fk_tbSystems_tbGovernment1_idx` (`government_id` ASC),
  INDEX `fk_tbSystems_tbState1_idx` (`state_id` ASC),
  INDEX `fk_tbSystems_tbSecurity1_idx` (`security_id` ASC),
  INDEX `idx_x` USING BTREE (`x` ASC),
  INDEX `idx_y` USING BTREE (`y` ASC),
  INDEX `idx_z` (`z` ASC),
  INDEX `idx_tbSystems_Systemname` (`systemname` ASC),
  INDEX `fk_tbSystems_tbPower1_idx` (`Power_id` ASC),
  INDEX `fk_tbSystems_tbPowerState1_idx` (`PowerState_id` ASC),
  INDEX `fk_tbSystems_tbVisitType1_idx` (`visited` ASC),
  CONSTRAINT `fk_tbSystems_tbAllegiance1`
    FOREIGN KEY (`allegiance_id`)
    REFERENCES `elite_db`.`tbAllegiance` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbSystems_tbEconomy1`
    FOREIGN KEY (`primary_economy_id`)
    REFERENCES `elite_db`.`tbEconomy` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbSystems_tbGovernment1`
    FOREIGN KEY (`government_id`)
    REFERENCES `elite_db`.`tbGovernment` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbSystems_tbState1`
    FOREIGN KEY (`state_id`)
    REFERENCES `elite_db`.`tbState` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbSystems_tbSecurity1`
    FOREIGN KEY (`security_id`)
    REFERENCES `elite_db`.`tbSecurity` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbSystems_tbPower1`
    FOREIGN KEY (`Power_id`)
    REFERENCES `elite_db`.`tbPower` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbSystems_tbPowerState1`
    FOREIGN KEY (`PowerState_id`)
    REFERENCES `elite_db`.`tbPowerState` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbSystems_tbVisitType1`
    FOREIGN KEY (`visited`)
    REFERENCES `elite_db`.`tbVisitType` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbStationType`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbStationType` (
  `id` INT NOT NULL,
  `stationtype` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbStations`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbStations` (
  `id` INT NOT NULL,
  `stationname` VARCHAR(80) NOT NULL,
  `system_id` INT NOT NULL,
  `max_landing_pad_size` VARCHAR(80) NULL,
  `distance_to_star` INT NULL,
  `faction` VARCHAR(80) NULL,
  `government_id` INT NULL,
  `allegiance_id` INT NULL,
  `state_id` INT NULL,
  `stationtype_id` INT NULL,
  `has_blackmarket` TINYINT(1) NULL,
  `has_market` TINYINT(1) NULL,
  `has_refuel` TINYINT(1) NULL,
  `has_repair` TINYINT(1) NULL,
  `has_rearm` TINYINT(1) NULL,
  `has_outfitting` TINYINT(1) NULL,
  `has_shipyard` TINYINT(1) NULL,
  `has_commodities` TINYINT(1) NULL,
  `is_planetary` TINYINT(1) NULL,
  `updated_at` DATETIME NOT NULL,
  `shipyard_updated_at` DATETIME NULL,
  `outfitting_updated_at` DATETIME NULL,
  `market_updated_at` DATETIME NULL,
  `is_changed` TINYINT(1) NULL DEFAULT 0,
  `visited` TINYINT NULL DEFAULT 0,
  `type_id` INT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_tbStations_tbSystems_idx` (`system_id` ASC),
  INDEX `fk_tbStations_tbAllegiance1_idx` (`allegiance_id` ASC),
  INDEX `fk_tbStations_tbGovernment1_idx` (`government_id` ASC),
  INDEX `fk_tbStations_tbState1_idx` (`state_id` ASC),
  INDEX `fk_tbStations_tbStationType1_idx` (`stationtype_id` ASC),
  INDEX `idx_tbStations_Stationname` (`stationname` ASC),
  INDEX `fk_tbStations_tbVisitType1_idx` (`visited` ASC),
  CONSTRAINT `fk_tbStations_tbSystems`
    FOREIGN KEY (`system_id`)
    REFERENCES `elite_db`.`tbSystems` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbStations_tbAllegiance1`
    FOREIGN KEY (`allegiance_id`)
    REFERENCES `elite_db`.`tbAllegiance` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbStations_tbGovernment1`
    FOREIGN KEY (`government_id`)
    REFERENCES `elite_db`.`tbGovernment` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbStations_tbState1`
    FOREIGN KEY (`state_id`)
    REFERENCES `elite_db`.`tbState` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbStations_tbStationType1`
    FOREIGN KEY (`stationtype_id`)
    REFERENCES `elite_db`.`tbStationType` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbStations_tbVisitType1`
    FOREIGN KEY (`visited`)
    REFERENCES `elite_db`.`tbVisitType` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbCategory`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbCategory` (
  `id` INT NOT NULL,
  `category` VARCHAR(80) NOT NULL,
  `loccategory` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbCommodity`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbCommodity` (
  `id` INT NOT NULL,
  `commodity` VARCHAR(80) NOT NULL,
  `loccommodity` VARCHAR(80) NULL,
  `category_id` INT NULL,
  `average_price` INT NULL DEFAULT -1,
  `pwl_demand_buy_low` INT NULL DEFAULT -1,
  `pwl_demand_buy_high` INT NULL DEFAULT -1,
  `pwl_supply_buy_low` INT NULL DEFAULT -1,
  `pwl_supply_buy_high` INT NULL DEFAULT -1,
  `pwl_demand_sell_low` INT NULL DEFAULT -1,
  `pwl_demand_sell_high` INT NULL DEFAULT -1,
  `pwl_supply_sell_low` INT NULL DEFAULT -1,
  `pwl_supply_sell_high` INT NULL DEFAULT -1,
  `is_rare` TINYINT(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`),
  INDEX `fk_tbCommodities_tbCategoriy1_idx` (`category_id` ASC),
  CONSTRAINT `fk_tbCommodities_tbCategoriy1`
    FOREIGN KEY (`category_id`)
    REFERENCES `elite_db`.`tbCategory` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbSource`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbSource` (
  `id` INT NOT NULL,
  `source` VARCHAR(80) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbEconomyLevel`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbEconomyLevel` (
  `id` INT NOT NULL,
  `level` VARCHAR(80) NOT NULL,
  `loclevel` VARCHAR(80) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbCommodityData`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbCommodityData` (
  `id` BIGINT(20) NOT NULL AUTO_INCREMENT,
  `station_id` INT NOT NULL,
  `commodity_id` INT NOT NULL,
  `Sell` INT NULL,
  `Buy` INT NULL,
  `Demand` INT NULL,
  `DemandLevel` INT NULL,
  `Supply` INT NULL,
  `SupplyLevel` INT NULL,
  `Sources_id` INT NOT NULL,
  `timestamp` DATETIME NOT NULL,
  INDEX `fk_tbStations_has_tbCommodities_tbCommodities1_idx` (`commodity_id` ASC),
  INDEX `fk_tbStations_has_tbCommodities_tbStations1_idx` (`station_id` ASC),
  PRIMARY KEY (`id`),
  INDEX `fk_tbStationCommodity_tbSources1_idx` (`Sources_id` ASC),
  INDEX `fk_tbStationCommodity_tbEconomyLevel1_idx` (`DemandLevel` ASC),
  INDEX `fk_tbStationCommodity_tbEconomyLevel2_idx` (`SupplyLevel` ASC),
  CONSTRAINT `fk_tbStations_has_tbCommodities_tbStations1`
    FOREIGN KEY (`station_id`)
    REFERENCES `elite_db`.`tbStations` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbStations_has_tbCommodities_tbCommodities1`
    FOREIGN KEY (`commodity_id`)
    REFERENCES `elite_db`.`tbCommodity` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbStationCommodity_tbSources1`
    FOREIGN KEY (`Sources_id`)
    REFERENCES `elite_db`.`tbSource` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbStationCommodity_tbEconomyLevel1`
    FOREIGN KEY (`DemandLevel`)
    REFERENCES `elite_db`.`tbEconomyLevel` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbStationCommodity_tbEconomyLevel2`
    FOREIGN KEY (`SupplyLevel`)
    REFERENCES `elite_db`.`tbEconomyLevel` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbPriceHistory`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbPriceHistory` (
  `id` BIGINT(20) NOT NULL AUTO_INCREMENT,
  `station_id` INT NOT NULL,
  `commodity_id` INT NOT NULL,
  `Sell` INT NULL,
  `Buy` INT NULL,
  `Demand` INT NULL,
  `DemandLevel` INT NULL,
  `Supply` INT NULL,
  `SupplyLevel` INT NULL,
  `Sources_id` INT NOT NULL,
  `timestamp` DATETIME NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_tbStations_has_tbCommodities1_tbCommodities1_idx` (`commodity_id` ASC),
  INDEX `fk_tbStations_has_tbCommodities1_tbStations1_idx` (`station_id` ASC),
  INDEX `fk_tbPriceHistory_tbSources1_idx` (`Sources_id` ASC),
  INDEX `fk_tbPriceHistory_tbEconomyLevel1_idx` (`DemandLevel` ASC),
  INDEX `fk_tbPriceHistory_tbEconomyLevel2_idx` (`SupplyLevel` ASC),
  CONSTRAINT `fk_tbStations_has_tbCommodities1_tbStations1`
    FOREIGN KEY (`station_id`)
    REFERENCES `elite_db`.`tbStations` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbStations_has_tbCommodities1_tbCommodities1`
    FOREIGN KEY (`commodity_id`)
    REFERENCES `elite_db`.`tbCommodity` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbPriceHistory_tbSources1`
    FOREIGN KEY (`Sources_id`)
    REFERENCES `elite_db`.`tbSource` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbPriceHistory_tbEconomyLevel1`
    FOREIGN KEY (`DemandLevel`)
    REFERENCES `elite_db`.`tbEconomyLevel` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbPriceHistory_tbEconomyLevel2`
    FOREIGN KEY (`SupplyLevel`)
    REFERENCES `elite_db`.`tbEconomyLevel` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbStationEconomy`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbStationEconomy` (
  `station_id` INT NOT NULL,
  `economy_id` INT NOT NULL,
  PRIMARY KEY (`station_id`, `economy_id`),
  INDEX `fk_tbStations_has_tbEconomy_tbEconomy1_idx` (`economy_id` ASC),
  INDEX `fk_tbStations_has_tbEconomy_tbStations1_idx` (`station_id` ASC),
  CONSTRAINT `fk_tbStations_has_tbEconomy_tbStations1`
    FOREIGN KEY (`station_id`)
    REFERENCES `elite_db`.`tbStations` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbStations_has_tbEconomy_tbEconomy1`
    FOREIGN KEY (`economy_id`)
    REFERENCES `elite_db`.`tbEconomy` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbLanguage`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbLanguage` (
  `id` INT NOT NULL,
  `language` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbCommodityLocalization`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbCommodityLocalization` (
  `commodity_id` INT NOT NULL,
  `language_id` INT NOT NULL,
  `locname` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`commodity_id`, `language_id`),
  INDEX `fk_tbLocalization_tbLanguage1_idx` (`language_id` ASC),
  CONSTRAINT `fk_tbLocalization_tbCommodities1`
    FOREIGN KEY (`commodity_id`)
    REFERENCES `elite_db`.`tbCommodity` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbLocalization_tbLanguage1`
    FOREIGN KEY (`language_id`)
    REFERENCES `elite_db`.`tbLanguage` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbCategoryLocalization`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbCategoryLocalization` (
  `category_id` INT NOT NULL,
  `language_id` INT NOT NULL,
  `locname` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`category_id`, `language_id`),
  INDEX `fk_tbLocalization_tbLanguage1_idx` (`language_id` ASC),
  CONSTRAINT `fk_tbLocalization_tbLanguage10`
    FOREIGN KEY (`language_id`)
    REFERENCES `elite_db`.`tbLanguage` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbCategoryLocalization_tbCategory1`
    FOREIGN KEY (`category_id`)
    REFERENCES `elite_db`.`tbCategory` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbInitValue`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbInitValue` (
  `InitGroup` VARCHAR(80) NOT NULL,
  `InitKey` VARCHAR(80) NOT NULL,
  `InitValue` VARCHAR(10000) NULL,
  PRIMARY KEY (`InitGroup`, `InitKey`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbEventType`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbEventType` (
  `id` INT NOT NULL,
  `eventtype` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbCargoAction`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbCargoAction` (
  `id` INT NOT NULL,
  `cargoaction` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbLog`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbLog` (
  `time` DATETIME NOT NULL,
  `system_id` INT NULL,
  `station_id` INT NULL,
  `event_id` INT NOT NULL,
  `commodity_id` INT NULL,
  `cargoaction_id` INT NULL,
  `cargovolume` INT NULL,
  `credits_transaction` INT NULL,
  `credits_total` INT NULL,
  `notes` VARCHAR(1024) NULL,
  `distance` DOUBLE NULL,
  INDEX `fk_tbLog_tbCommodities1_idx` (`commodity_id` ASC),
  INDEX `fk_tbLog_tbSystems1_idx` (`system_id` ASC),
  INDEX `fk_tbLog_tbStations1_idx` (`station_id` ASC),
  INDEX `fk_tbLog_tbEventType1_idx` (`event_id` ASC),
  INDEX `fk_tbLog_tbCargoAction1_idx` (`cargoaction_id` ASC),
  PRIMARY KEY (`time`),
  CONSTRAINT `fk_tbLog_tbCommodities1`
    FOREIGN KEY (`commodity_id`)
    REFERENCES `elite_db`.`tbCommodity` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbLog_tbSystems1`
    FOREIGN KEY (`system_id`)
    REFERENCES `elite_db`.`tbSystems` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbLog_tbStations1`
    FOREIGN KEY (`station_id`)
    REFERENCES `elite_db`.`tbStations` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbLog_tbEventType1`
    FOREIGN KEY (`event_id`)
    REFERENCES `elite_db`.`tbEventType` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbLog_tbCargoAction1`
    FOREIGN KEY (`cargoaction_id`)
    REFERENCES `elite_db`.`tbCargoAction` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbVisitedSystems`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbVisitedSystems` (
  `system_id` INT NOT NULL,
  `time` DATETIME NOT NULL,
  `visitType` TINYINT NOT NULL DEFAULT 2,
  PRIMARY KEY (`system_id`),
  INDEX `fk_tbVisitedSystems_tbVisitType1_idx` (`visitType` ASC),
  INDEX `Idx_tbVisitedSystems_time` (`time` ASC),
  CONSTRAINT `fk_tbSystems_tbVisitedSystems`
    FOREIGN KEY (`system_id`)
    REFERENCES `elite_db`.`tbSystems` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbVisitedSystems_tbVisitType1`
    FOREIGN KEY (`visitType`)
    REFERENCES `elite_db`.`tbVisitType` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbVisitedStations`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbVisitedStations` (
  `station_id` INT NOT NULL,
  `time` DATETIME NOT NULL,
  `visitType` TINYINT NOT NULL DEFAULT 2,
  PRIMARY KEY (`station_id`),
  INDEX `fk_tbVisitedStations_tbVisitType1_idx` (`visitType` ASC),
  INDEX `Idx_tbVisitedStations_time` (`time` ASC),
  CONSTRAINT `fk_tbStations_tbVisitedStations`
    FOREIGN KEY (`station_id`)
    REFERENCES `elite_db`.`tbStations` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbVisitedStations_tbVisitType1`
    FOREIGN KEY (`visitType`)
    REFERENCES `elite_db`.`tbVisitType` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbAttribute`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbAttribute` (
  `id` INT NOT NULL,
  `Attribute` VARCHAR(80) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbCommodityClassification`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbCommodityClassification` (
  `id` INT UNSIGNED NOT NULL,
  `station_id` INT NOT NULL,
  `commodity_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_tbCommodityClassification_tbStations1_idx` (`station_id` ASC),
  INDEX `fk_tbCommodityClassification_tbCommodity1_idx` (`commodity_id` ASC),
  CONSTRAINT `fk_tbCommodityClassification_tbStations1`
    FOREIGN KEY (`station_id`)
    REFERENCES `elite_db`.`tbStations` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbCommodityClassification_tbCommodity1`
    FOREIGN KEY (`commodity_id`)
    REFERENCES `elite_db`.`tbCommodity` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbCommodity_has_Attribute`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbCommodity_has_Attribute` (
  `tbAttribute_id` INT NOT NULL,
  `tbCommodityClassification_id` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`tbAttribute_id`, `tbCommodityClassification_id`),
  INDEX `fk_tbCommodityAttribute_has_tbStationCommodity_tbCommodityA_idx` (`tbAttribute_id` ASC),
  INDEX `fk_tbCommodity_has_Attribute_tbCommodityClassification1_idx` (`tbCommodityClassification_id` ASC),
  CONSTRAINT `fk_tbCommodityAttribute_has_tbStationCommodity_tbCommodityAtt1`
    FOREIGN KEY (`tbAttribute_id`)
    REFERENCES `elite_db`.`tbAttribute` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbCommodity_has_Attribute_tbCommodityClassification1`
    FOREIGN KEY (`tbCommodityClassification_id`)
    REFERENCES `elite_db`.`tbCommodityClassification` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbLevelLocalization`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbLevelLocalization` (
  `economylevel_id` INT NOT NULL,
  `language_id` INT NOT NULL,
  `locname` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`economylevel_id`, `language_id`),
  INDEX `fk_tbEconomyLevel_has_tbLanguage_tbLanguage1_idx` (`language_id` ASC),
  INDEX `fk_tbEconomyLevel_has_tbLanguage_tbEconomyLevel1_idx` (`economylevel_id` ASC),
  CONSTRAINT `fk_tbEconomyLevel_has_tbLanguage_tbEconomyLevel1`
    FOREIGN KEY (`economylevel_id`)
    REFERENCES `elite_db`.`tbEconomyLevel` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbEconomyLevel_has_tbLanguage_tbLanguage1`
    FOREIGN KEY (`language_id`)
    REFERENCES `elite_db`.`tbLanguage` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tmFilteredStations`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tmFilteredStations` (
  `System_id` INT NOT NULL,
  `Station_id` INT NOT NULL,
  `Distance` DOUBLE NULL,
  `x` DOUBLE NULL,
  `y` DOUBLE NULL,
  `z` DOUBLE NULL,
  PRIMARY KEY (`System_id`, `Station_id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tmPA_AllCommodities`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tmPA_AllCommodities` (
  `CommodityID` INT NOT NULL,
  `Commodity` VARCHAR(80) NULL,
  `Buy_SystemID` INT NULL,
  `Buy_System` VARCHAR(80) NULL,
  `Buy_StationID` INT NULL,
  `Buy_Station` VARCHAR(80) NULL,
  `Buy_Min` INT NULL,
  `Buy_Distance` DOUBLE NULL,
  `Buy_Timestamp` DATETIME NULL,
  `Buy_Sources_id` INT NULL,
  `Sell_SystemID` INT NULL,
  `Sell_System` VARCHAR(80) NULL,
  `Sell_StationID` INT NULL,
  `Sell_Station` VARCHAR(80) NULL,
  `Sell_Max` INT NULL,
  `Sell_Distance` DOUBLE NULL,
  `Sell_Timestamp` DATETIME NULL,
  `Sell_Sources_id` INT NULL,
  `Max_Profit` INT NULL,
  PRIMARY KEY (`CommodityID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tmNeighbourStations`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tmNeighbourStations` (
  `System_ID_From` INT NULL,
  `Station_ID_From` INT NOT NULL,
  `Distance_From` DOUBLE NULL,
  `System_ID_To` INT NULL,
  `Station_ID_To` INT NOT NULL,
  `Distance_To` DOUBLE NULL,
  `Distance_Between` DOUBLE NULL,
  PRIMARY KEY (`Station_ID_From`, `Station_ID_To`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tmPA_S2S_StationData`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tmPA_S2S_StationData` (
  `Station_ID` INT(11) NULL,
  `Commodity_ID` INT NOT NULL,
  `Commodity` VARCHAR(80) NULL,
  `Buy` INT NULL,
  `Supply` INT NULL,
  `SupplyLevel` VARCHAR(10) NULL,
  `Timestamp1` DATETIME NULL,
  `Sell` INT NULL,
  `Demand` INT NULL,
  `Demandlevel` VARCHAR(10) NULL,
  `Timestamp2` DATETIME NULL,
  `Profit` INT NULL,
  `Sources_id` INT(11) NULL DEFAULT NULL,
  PRIMARY KEY (`Commodity_ID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tmPA_S2S_BestTrips`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tmPA_S2S_BestTrips` (
  `System_ID_1` INT NULL,
  `SystemName_1` VARCHAR(80) NULL,
  `Station_ID_1` INT NOT NULL,
  `StationName_1` VARCHAR(80) NULL,
  `TimeStamp_1` DATETIME NULL,
  `Station_Location_1` VARCHAR(80) NULL,
  `System_ID_2` INT NULL,
  `SystemName_2` VARCHAR(80) NULL,
  `Station_ID_2` INT NOT NULL,
  `StationName_2` VARCHAR(80) NULL,
  `TimeStamp_2` DATETIME NULL,
  `Station_Location_2` VARCHAR(80) NULL,
  `Profit` INT NULL,
  `Distance` DOUBLE NULL,
  `DistanceToStar_1` DOUBLE NULL,
  `DistanceToStar_2` DOUBLE NULL,
  `DistanceToRoute` DOUBLE NULL,
  PRIMARY KEY (`Station_ID_1`, `Station_ID_2`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tmBestProfits`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tmBestProfits` (
  `Station_Id_From` INT NOT NULL,
  `Station_Id_To` INT NOT NULL,
  `Max_Profit` INT NULL,
  PRIMARY KEY (`Station_Id_From`, `Station_Id_To`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tmPA_ByStation`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tmPA_ByStation` (
  `Station_ID` INT(11) NULL,
  `Commodity_ID` INT NOT NULL,
  `Commodity` VARCHAR(80) NULL,
  `Buy` INT NULL,
  `Supply` INT NULL,
  `SupplyLevel` VARCHAR(80) NULL,
  `Sell` INT NULL,
  `Demand` INT NULL,
  `DemandLevel` VARCHAR(80) NULL,
  `Timestamp` DATETIME NULL,
  `Best_Buy` INT NULL,
  `Best_Sell` INT NULL,
  `MaxProfit` INT NULL,
  `Sources_id` INT(11) NULL DEFAULT NULL,
  PRIMARY KEY (`Commodity_ID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tmPA_ByCommodity`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tmPA_ByCommodity` (
  `System_ID` INT NULL,
  `System` VARCHAR(80) NULL,
  `Station_ID` INT NOT NULL,
  `Station` VARCHAR(80) NULL,
  `Distance` DOUBLE NULL,
  `Buy` INT NULL,
  `Supply` INT NULL,
  `SupplyLevel` VARCHAR(80) NULL,
  `Sell` INT NULL,
  `Demand` INT NULL,
  `DemandLevel` VARCHAR(80) NULL,
  `Timestamp` DATETIME NULL,
  `Sources_id` INT(11) NULL DEFAULT NULL,
  PRIMARY KEY (`Station_ID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbSystems_org`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbSystems_org` (
  `id` INT NOT NULL,
  `systemname` VARCHAR(80) NOT NULL,
  `x` DOUBLE NULL,
  `y` DOUBLE NULL,
  `z` DOUBLE NULL,
  `faction` VARCHAR(80) NULL,
  `population` MEDIUMTEXT NULL,
  `government_id` INT NULL,
  `allegiance_id` INT NULL,
  `state_id` INT NULL,
  `security_id` INT NULL,
  `primary_economy_id` INT NULL,
  `needs_permit` TINYINT(1) NULL,
  `updated_at` DATETIME NOT NULL,
  `is_changed` TINYINT(1) NULL DEFAULT 0,
  `visited` TINYINT(1) NOT NULL DEFAULT 0,
  `Power_id` INT NULL,
  `PowerState_id` INT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_tbSystems_tbAllegiance1_idx` (`allegiance_id` ASC),
  INDEX `fk_tbSystems_tbEconomy1_idx` (`primary_economy_id` ASC),
  INDEX `fk_tbSystems_tbGovernment1_idx` (`government_id` ASC),
  INDEX `fk_tbSystems_tbState1_idx` (`state_id` ASC),
  INDEX `fk_tbSystems_tbSecurity1_idx` (`security_id` ASC),
  INDEX `idx_x` USING BTREE (`x` ASC),
  INDEX `idx_y` USING BTREE (`y` ASC),
  INDEX `idx_z` (`z` ASC),
  INDEX `idx_tbSystems_Systemname` (`systemname` ASC),
  INDEX `fk_tbSystems_tbPower1_idx` (`Power_id` ASC),
  INDEX `fk_tbSystems_tbPowerState1_idx` (`PowerState_id` ASC),
  CONSTRAINT `fk_tbSystems_tbAllegiance10`
    FOREIGN KEY (`allegiance_id`)
    REFERENCES `elite_db`.`tbAllegiance` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbSystems_tbEconomy10`
    FOREIGN KEY (`primary_economy_id`)
    REFERENCES `elite_db`.`tbEconomy` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbSystems_tbGovernment10`
    FOREIGN KEY (`government_id`)
    REFERENCES `elite_db`.`tbGovernment` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbSystems_tbState10`
    FOREIGN KEY (`state_id`)
    REFERENCES `elite_db`.`tbState` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbSystems_tbSecurity10`
    FOREIGN KEY (`security_id`)
    REFERENCES `elite_db`.`tbSecurity` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbSystems_tbPower10`
    FOREIGN KEY (`Power_id`)
    REFERENCES `elite_db`.`tbPower` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbSystems_tbPowerState10`
    FOREIGN KEY (`PowerState_id`)
    REFERENCES `elite_db`.`tbPowerState` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbStations_org`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbStations_org` (
  `id` INT NOT NULL,
  `stationname` VARCHAR(80) NOT NULL,
  `system_id` INT NOT NULL,
  `max_landing_pad_size` VARCHAR(80) NULL,
  `distance_to_star` INT NULL,
  `faction` VARCHAR(80) NULL,
  `government_id` INT NULL,
  `allegiance_id` INT NULL,
  `state_id` INT NULL,
  `stationtype_id` INT NULL,
  `has_blackmarket` TINYINT(1) NULL,
  `has_market` TINYINT(1) NULL,
  `has_refuel` TINYINT(1) NULL,
  `has_repair` TINYINT(1) NULL,
  `has_rearm` TINYINT(1) NULL,
  `has_outfitting` TINYINT(1) NULL,
  `has_shipyard` TINYINT(1) NULL,
  `has_commodities` TINYINT(1) NULL,
  `is_planetary` TINYINT(1) NULL,
  `updated_at` DATETIME NOT NULL,
  `shipyard_updated_at` DATETIME NULL,
  `outfitting_updated_at` DATETIME NULL,
  `market_updated_at` DATETIME NULL,
  `is_changed` TINYINT(1) NULL DEFAULT 0,
  `visited` TINYINT(1) NULL DEFAULT 0,
  `type_id` INT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_tbStations_tbSystems_idx` (`system_id` ASC),
  INDEX `fk_tbStations_tbAllegiance1_idx` (`allegiance_id` ASC),
  INDEX `fk_tbStations_tbGovernment1_idx` (`government_id` ASC),
  INDEX `fk_tbStations_tbState1_idx` (`state_id` ASC),
  INDEX `fk_tbStations_tbStationType1_idx` (`stationtype_id` ASC),
  INDEX `idx_tbStations_Stationname` (`stationname` ASC),
  CONSTRAINT `fk_tbStations_tbSystems0`
    FOREIGN KEY (`system_id`)
    REFERENCES `elite_db`.`tbSystems` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbStations_tbAllegiance10`
    FOREIGN KEY (`allegiance_id`)
    REFERENCES `elite_db`.`tbAllegiance` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbStations_tbGovernment10`
    FOREIGN KEY (`government_id`)
    REFERENCES `elite_db`.`tbGovernment` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbStations_tbState10`
    FOREIGN KEY (`state_id`)
    REFERENCES `elite_db`.`tbState` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbStations_tbStationType10`
    FOREIGN KEY (`stationtype_id`)
    REFERENCES `elite_db`.`tbStationType` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbTrustedSenders`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbTrustedSenders` (
  `Name` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`Name`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbDNMap_Commodity`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbDNMap_Commodity` (
  `CompanionName` VARCHAR(255) NOT NULL,
  `CompanionAddition` VARCHAR(255) NOT NULL,
  `GameName` VARCHAR(255) NOT NULL,
  `GameAddition` VARCHAR(255) NOT NULL,
  `ts` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`CompanionName`, `CompanionAddition`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbOutfittingBase`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbOutfittingBase` (
  `id` INT NOT NULL,
  `symbol` VARCHAR(80) NOT NULL,
  `category` VARCHAR(80) NOT NULL,
  `name` VARCHAR(80) NOT NULL,
  `mount` VARCHAR(80) NULL,
  `guidance` VARCHAR(80) NULL,
  `ship` VARCHAR(80) NULL,
  `class` CHAR(1) NOT NULL,
  `rating` CHAR(1) NOT NULL,
  `entitlement` VARCHAR(80) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbCommodityBase`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbCommodityBase` (
  `id` INT NOT NULL,
  `symbol` VARCHAR(80) NULL,
  `category` VARCHAR(80) NULL,
  `name` VARCHAR(80) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbShipyardBase`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbShipyardBase` (
  `id` INT NOT NULL,
  `symbol` VARCHAR(80) NOT NULL,
  `name` VARCHAR(80) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `elite_db`.`tbEDDNRelays`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`tbEDDNRelays` (
  `Address` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`Address`))
ENGINE = InnoDB;

USE `elite_db` ;

-- -----------------------------------------------------
-- Placeholder table for view `elite_db`.`vilog`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`vilog` (`time` INT, `systemname` INT, `stationname` INT, `eventtype` INT, `cargoaction` INT, `loccommodity` INT, `cargovolume` INT, `credits_transaction` INT, `credits_total` INT, `distance` INT, `notes` INT);

-- -----------------------------------------------------
-- Placeholder table for view `elite_db`.`viSystemsAndStations`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `elite_db`.`viSystemsAndStations` (`SystemID` INT, `SystemName` INT, `StationID` INT, `StationName` INT);

-- -----------------------------------------------------
-- View `elite_db`.`vilog`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `elite_db`.`vilog`;
USE `elite_db`;
CREATE  OR REPLACE ALGORITHM=UNDEFINED DEFINER=`root`@`127.0.0.1` SQL SECURITY DEFINER VIEW `vilog` AS 
select `l`.`time` AS `time`,`s`.`systemname` AS `systemname`,`st`.`stationname` AS `stationname`,`e`.`eventtype` AS `eventtype`,`c`.`cargoaction` 
AS `cargoaction`,`co`.`loccommodity` AS `loccommodity`,`l`.`cargovolume` AS `cargovolume`,`l`.`credits_transaction` 
AS `credits_transaction`,`l`.`credits_total` AS `credits_total`, `l`.`distance` AS `distance`, `l`.`notes` AS `notes` from (((((`tblog` `l` 
left join `tbeventtype` `e` on((`l`.`event_id` = `e`.`id`))) left join `tbcargoaction` `c` on((`l`.`cargoaction_id` = `c`.`id`))) 
left join `tbsystems` `s` on((`l`.`system_id` = `s`.`id`))) left join `tbstations` `st` on((`l`.`station_id` = `st`.`id`))) 
left join `tbcommodity` `co` on((`l`.`commodity_id` = `co`.`id`)));

-- -----------------------------------------------------
-- View `elite_db`.`viSystemsAndStations`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `elite_db`.`viSystemsAndStations`;
USE `elite_db`;
CREATE  OR REPLACE ALGORITHM=UNDEFINED DEFINER=`root`@`127.0.0.1` SQL SECURITY DEFINER VIEW `visystemsandstations` AS select `sy`.`id` AS `SystemID`,`sy`.`systemname` AS `SystemName`,`s`.`id` AS `StationID`,`s`.`stationname` AS `StationName` from (`tbstations` `s` join `tbsystems` `sy`) where (`s`.`system_id` = `sy`.`id`);
USE `elite_db`;

DELIMITER $$
USE `elite_db`$$
CREATE
DEFINER=`RN_User`@`::1`
TRIGGER `elite_db`.`tbCommodityData_AFTER_INSERT`
AFTER INSERT ON `elite_db`.`tbcommoditydata`
FOR EACH ROW
BEGIN                                                                                                                                                                         
	DECLARE isActive BOOLEAN;                                                                                                                                                  
                                                                                                                                                                              
    SELECT ((InitValue <> '0') and (InitValue <> 'False')) INTO isActive                                                                                                      
    FROM tbInitValue                                                                                                                                                          
    WHERE InitGroup = 'Database'                                                                                                                                              
    AND   InitKey   = 'CollectPriceHistory';                                                                                                                                  
                                                                                                                                                                              
    IF isActive THEN                                                                                                                                                          
		INSERT INTO `elite_db`.`tbPriceHistory`                                                                                                                                
		(`station_id`, `commodity_id`, `Sell`, `Buy`, `Demand`, `DemandLevel`, `Supply`, `SupplyLevel`, `Sources_id`, `timestamp`)                                             
		VALUES                                                                                                                                                                 
		(NEW.`station_id`, NEW.`commodity_id`, NEW.`Sell`, NEW.`Buy`, NEW.`Demand`, NEW.`DemandLevel`, NEW.`Supply`, NEW.`SupplyLevel`, NEW.`Sources_id`, NEW.`timestamp`);	   
	END IF;                                                                                                                                                                    
END$$

USE `elite_db`$$
CREATE
DEFINER=`RN_User`@`::1`
TRIGGER `elite_db`.`tbCommodityData_AFTER_UPDATE`
AFTER UPDATE ON `elite_db`.`tbcommoditydata`
FOR EACH ROW
BEGIN                                                                                                                                                                         
	DECLARE isActive BOOLEAN;                                                                                                                                                  
                                                                                                                                                                              
    SELECT ((InitValue <> '0') and (InitValue <> 'False')) INTO isActive                                                                                                      
    FROM tbInitValue                                                                                                                                                          
    WHERE InitGroup = 'Database'                                                                                                                                              
    AND   InitKey   = 'CollectPriceHistory';                                                                                                                                  
                                                                                                                                                                              
    IF isActive THEN                                                                                                                                                          
		IF (NEW.Sell <> OLD.Sell) OR (NEW.Buy <> OLD.Buy) OR (NEW.Sources_id <> OLD.Sources_id) OR                                                                             
		   (TIMESTAMPDIFF(hour, OLD.timestamp, NEW.timestamp) > 24) THEN                                                                                                       
			INSERT INTO `elite_db`.`tbPriceHistory`                                                                                                                            
			(`station_id`, `commodity_id`, `Sell`, `Buy`, `Demand`, `DemandLevel`, `Supply`, `SupplyLevel`, `Sources_id`, `timestamp`)                                         
			VALUES                                                                                                                                                             
			(NEW.`station_id`, NEW.`commodity_id`, NEW.`Sell`, NEW.`Buy`, NEW.`Demand`, NEW.`DemandLevel`, NEW.`Supply`, NEW.`SupplyLevel`, NEW.`Sources_id`, NEW.`timestamp`);
		END IF;                                                                                                                                                                
	END IF;                                                                                                                                                                    
END$$


DELIMITER ;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

-- -----------------------------------------------------
-- Data for table `elite_db`.`tbAllegiance`
-- -----------------------------------------------------
START TRANSACTION;
USE `elite_db`;
INSERT INTO `elite_db`.`tbAllegiance` (`id`, `allegiance`) VALUES (0, 'Alliance');
INSERT INTO `elite_db`.`tbAllegiance` (`id`, `allegiance`) VALUES (1, 'Anarchy');
INSERT INTO `elite_db`.`tbAllegiance` (`id`, `allegiance`) VALUES (2, 'Empire');
INSERT INTO `elite_db`.`tbAllegiance` (`id`, `allegiance`) VALUES (3, 'Federation');
INSERT INTO `elite_db`.`tbAllegiance` (`id`, `allegiance`) VALUES (4, 'Independent');
INSERT INTO `elite_db`.`tbAllegiance` (`id`, `allegiance`) VALUES (5, 'None');

COMMIT;


-- -----------------------------------------------------
-- Data for table `elite_db`.`tbEconomy`
-- -----------------------------------------------------
START TRANSACTION;
USE `elite_db`;
INSERT INTO `elite_db`.`tbEconomy` (`id`, `economy`) VALUES (0, 'Agriculture');
INSERT INTO `elite_db`.`tbEconomy` (`id`, `economy`) VALUES (1, 'Extraction');
INSERT INTO `elite_db`.`tbEconomy` (`id`, `economy`) VALUES (2, 'High Tech');
INSERT INTO `elite_db`.`tbEconomy` (`id`, `economy`) VALUES (3, 'Industrial');
INSERT INTO `elite_db`.`tbEconomy` (`id`, `economy`) VALUES (4, 'Military');
INSERT INTO `elite_db`.`tbEconomy` (`id`, `economy`) VALUES (5, 'Refinery');
INSERT INTO `elite_db`.`tbEconomy` (`id`, `economy`) VALUES (6, 'Service');
INSERT INTO `elite_db`.`tbEconomy` (`id`, `economy`) VALUES (7, 'Terraforming');
INSERT INTO `elite_db`.`tbEconomy` (`id`, `economy`) VALUES (8, 'Tourism');
INSERT INTO `elite_db`.`tbEconomy` (`id`, `economy`) VALUES (9, 'None');

COMMIT;


-- -----------------------------------------------------
-- Data for table `elite_db`.`tbGovernment`
-- -----------------------------------------------------
START TRANSACTION;
USE `elite_db`;
INSERT INTO `elite_db`.`tbGovernment` (`id`, `government`) VALUES (0, 'None');
INSERT INTO `elite_db`.`tbGovernment` (`id`, `government`) VALUES (1, 'Anarchy');
INSERT INTO `elite_db`.`tbGovernment` (`id`, `government`) VALUES (2, 'Communism');
INSERT INTO `elite_db`.`tbGovernment` (`id`, `government`) VALUES (3, 'Confederacy');
INSERT INTO `elite_db`.`tbGovernment` (`id`, `government`) VALUES (4, 'Corporate');
INSERT INTO `elite_db`.`tbGovernment` (`id`, `government`) VALUES (5, 'Cooperative');
INSERT INTO `elite_db`.`tbGovernment` (`id`, `government`) VALUES (6, 'Democracy');
INSERT INTO `elite_db`.`tbGovernment` (`id`, `government`) VALUES (7, 'Dictatorship');
INSERT INTO `elite_db`.`tbGovernment` (`id`, `government`) VALUES (8, 'Feudal');
INSERT INTO `elite_db`.`tbGovernment` (`id`, `government`) VALUES (9, 'Imperial');
INSERT INTO `elite_db`.`tbGovernment` (`id`, `government`) VALUES (10, 'Patronage');
INSERT INTO `elite_db`.`tbGovernment` (`id`, `government`) VALUES (11, 'Colony');
INSERT INTO `elite_db`.`tbGovernment` (`id`, `government`) VALUES (12, 'Prison Colony');
INSERT INTO `elite_db`.`tbGovernment` (`id`, `government`) VALUES (13, 'Theocracy');

COMMIT;


-- -----------------------------------------------------
-- Data for table `elite_db`.`tbState`
-- -----------------------------------------------------
START TRANSACTION;
USE `elite_db`;
INSERT INTO `elite_db`.`tbState` (`id`, `state`) VALUES (0, 'Boom');
INSERT INTO `elite_db`.`tbState` (`id`, `state`) VALUES (1, 'Bust');
INSERT INTO `elite_db`.`tbState` (`id`, `state`) VALUES (2, 'Civil Unrest');
INSERT INTO `elite_db`.`tbState` (`id`, `state`) VALUES (3, 'Civil War');
INSERT INTO `elite_db`.`tbState` (`id`, `state`) VALUES (4, 'Expansion');
INSERT INTO `elite_db`.`tbState` (`id`, `state`) VALUES (5, 'Lockdown');
INSERT INTO `elite_db`.`tbState` (`id`, `state`) VALUES (6, 'Outbreak');
INSERT INTO `elite_db`.`tbState` (`id`, `state`) VALUES (7, 'War');
INSERT INTO `elite_db`.`tbState` (`id`, `state`) VALUES (8, 'None');

COMMIT;


-- -----------------------------------------------------
-- Data for table `elite_db`.`tbSecurity`
-- -----------------------------------------------------
START TRANSACTION;
USE `elite_db`;
INSERT INTO `elite_db`.`tbSecurity` (`id`, `security`) VALUES (0, 'Low');
INSERT INTO `elite_db`.`tbSecurity` (`id`, `security`) VALUES (1, 'Medium');
INSERT INTO `elite_db`.`tbSecurity` (`id`, `security`) VALUES (2, 'High');

COMMIT;


-- -----------------------------------------------------
-- Data for table `elite_db`.`tbVisitType`
-- -----------------------------------------------------
START TRANSACTION;
USE `elite_db`;
INSERT INTO `elite_db`.`tbVisitType` (`id`, `VisitType`) VALUES (0, 'NoVisit');
INSERT INTO `elite_db`.`tbVisitType` (`id`, `VisitType`) VALUES (1, 'VirtualVisit');
INSERT INTO `elite_db`.`tbVisitType` (`id`, `VisitType`) VALUES (2, 'RealVisit');

COMMIT;


-- -----------------------------------------------------
-- Data for table `elite_db`.`tbStationType`
-- -----------------------------------------------------
START TRANSACTION;
USE `elite_db`;
INSERT INTO `elite_db`.`tbStationType` (`id`, `stationtype`) VALUES (0, 'Civilian Outpost');
INSERT INTO `elite_db`.`tbStationType` (`id`, `stationtype`) VALUES (1, 'Commercial Outpost');
INSERT INTO `elite_db`.`tbStationType` (`id`, `stationtype`) VALUES (2, 'Coriolis Starport');
INSERT INTO `elite_db`.`tbStationType` (`id`, `stationtype`) VALUES (3, 'Industrial Outpost');
INSERT INTO `elite_db`.`tbStationType` (`id`, `stationtype`) VALUES (4, 'Military Outpost');
INSERT INTO `elite_db`.`tbStationType` (`id`, `stationtype`) VALUES (5, 'Mining Outpost');
INSERT INTO `elite_db`.`tbStationType` (`id`, `stationtype`) VALUES (6, 'Ocellus Starport');
INSERT INTO `elite_db`.`tbStationType` (`id`, `stationtype`) VALUES (7, 'Orbis Starport');
INSERT INTO `elite_db`.`tbStationType` (`id`, `stationtype`) VALUES (8, 'Scientific Outpost');
INSERT INTO `elite_db`.`tbStationType` (`id`, `stationtype`) VALUES (9, 'Unsanctioned Outpost');
INSERT INTO `elite_db`.`tbStationType` (`id`, `stationtype`) VALUES (10, 'Unknown Outpost');
INSERT INTO `elite_db`.`tbStationType` (`id`, `stationtype`) VALUES (11, 'Unknown Starport');

COMMIT;


-- -----------------------------------------------------
-- Data for table `elite_db`.`tbSource`
-- -----------------------------------------------------
START TRANSACTION;
USE `elite_db`;
INSERT INTO `elite_db`.`tbSource` (`id`, `source`) VALUES (1, 'IBE');
INSERT INTO `elite_db`.`tbSource` (`id`, `source`) VALUES (2, 'EDDN');
INSERT INTO `elite_db`.`tbSource` (`id`, `source`) VALUES (3, 'FILE');

COMMIT;


-- -----------------------------------------------------
-- Data for table `elite_db`.`tbEconomyLevel`
-- -----------------------------------------------------
START TRANSACTION;
USE `elite_db`;
INSERT INTO `elite_db`.`tbEconomyLevel` (`id`, `level`, `loclevel`) VALUES (0, 'low', '');
INSERT INTO `elite_db`.`tbEconomyLevel` (`id`, `level`, `loclevel`) VALUES (1, 'med', '');
INSERT INTO `elite_db`.`tbEconomyLevel` (`id`, `level`, `loclevel`) VALUES (2, 'high', '');

COMMIT;


-- -----------------------------------------------------
-- Data for table `elite_db`.`tbLanguage`
-- -----------------------------------------------------
START TRANSACTION;
USE `elite_db`;
INSERT INTO `elite_db`.`tbLanguage` (`id`, `language`) VALUES (0, 'eng');
INSERT INTO `elite_db`.`tbLanguage` (`id`, `language`) VALUES (1, 'ger');
INSERT INTO `elite_db`.`tbLanguage` (`id`, `language`) VALUES (2, 'fra');

COMMIT;


-- -----------------------------------------------------
-- Data for table `elite_db`.`tbInitValue`
-- -----------------------------------------------------
START TRANSACTION;
USE `elite_db`;
INSERT INTO `elite_db`.`tbInitValue` (`InitGroup`, `InitKey`, `InitValue`) VALUES ('Database', 'Version', '0.7.0');
INSERT INTO `elite_db`.`tbInitValue` (`InitGroup`, `InitKey`, `InitValue`) VALUES ('Database', 'CollectPriceHistory', 'False');

COMMIT;


-- -----------------------------------------------------
-- Data for table `elite_db`.`tbEventType`
-- -----------------------------------------------------
START TRANSACTION;
USE `elite_db`;
INSERT INTO `elite_db`.`tbEventType` (`id`, `eventtype`) VALUES (1, 'Jumped To');
INSERT INTO `elite_db`.`tbEventType` (`id`, `eventtype`) VALUES (2, 'Visited');
INSERT INTO `elite_db`.`tbEventType` (`id`, `eventtype`) VALUES (3, 'Market Data Collected');
INSERT INTO `elite_db`.`tbEventType` (`id`, `eventtype`) VALUES (4, 'Cargo');
INSERT INTO `elite_db`.`tbEventType` (`id`, `eventtype`) VALUES (5, 'Fight');
INSERT INTO `elite_db`.`tbEventType` (`id`, `eventtype`) VALUES (6, 'Docked');
INSERT INTO `elite_db`.`tbEventType` (`id`, `eventtype`) VALUES (7, 'Took Off');
INSERT INTO `elite_db`.`tbEventType` (`id`, `eventtype`) VALUES (8, 'Saved Game');
INSERT INTO `elite_db`.`tbEventType` (`id`, `eventtype`) VALUES (9, 'Loaded Game');
INSERT INTO `elite_db`.`tbEventType` (`id`, `eventtype`) VALUES (10, 'Mission Accepted ');
INSERT INTO `elite_db`.`tbEventType` (`id`, `eventtype`) VALUES (11, 'Mission Completed');
INSERT INTO `elite_db`.`tbEventType` (`id`, `eventtype`) VALUES (12, 'Other');
INSERT INTO `elite_db`.`tbEventType` (`id`, `eventtype`) VALUES (13, 'Resurrect');
INSERT INTO `elite_db`.`tbEventType` (`id`, `eventtype`) VALUES (14, 'Died');
INSERT INTO `elite_db`.`tbEventType` (`id`, `eventtype`) VALUES (15, 'Touchdown');
INSERT INTO `elite_db`.`tbEventType` (`id`, `eventtype`) VALUES (16, 'Liftoff');
INSERT INTO `elite_db`.`tbEventType` (`id`, `eventtype`) VALUES (17, 'Scan');
INSERT INTO `elite_db`.`tbEventType` (`id`, `eventtype`) VALUES (18, 'Mission Abandoned');
INSERT INTO `elite_db`.`tbEventType` (`id`, `eventtype`) VALUES (19, 'Mission Failed');
INSERT INTO `elite_db`.`tbEventType` (`id`, `eventtype`) VALUES (20, 'Load Game');

COMMIT;


-- -----------------------------------------------------
-- Data for table `elite_db`.`tbCargoAction`
-- -----------------------------------------------------
START TRANSACTION;
USE `elite_db`;
INSERT INTO `elite_db`.`tbCargoAction` (`id`, `cargoaction`) VALUES (1, 'Bought');
INSERT INTO `elite_db`.`tbCargoAction` (`id`, `cargoaction`) VALUES (2, 'Sold');
INSERT INTO `elite_db`.`tbCargoAction` (`id`, `cargoaction`) VALUES (3, 'Mined');
INSERT INTO `elite_db`.`tbCargoAction` (`id`, `cargoaction`) VALUES (4, 'Stolen');
INSERT INTO `elite_db`.`tbCargoAction` (`id`, `cargoaction`) VALUES (5, 'Found');

COMMIT;


-- -----------------------------------------------------
-- Data for table `elite_db`.`tbAttribute`
-- -----------------------------------------------------
START TRANSACTION;
USE `elite_db`;
INSERT INTO `elite_db`.`tbAttribute` (`id`, `Attribute`) VALUES (0, 'import');
INSERT INTO `elite_db`.`tbAttribute` (`id`, `Attribute`) VALUES (1, 'export');
INSERT INTO `elite_db`.`tbAttribute` (`id`, `Attribute`) VALUES (2, 'prohibited');

COMMIT;


-- -----------------------------------------------------
-- Data for table `elite_db`.`tbTrustedSenders`
-- -----------------------------------------------------
START TRANSACTION;
USE `elite_db`;
INSERT INTO `elite_db`.`tbTrustedSenders` (`Name`) VALUES ('E:D Market Connector [Windows]');
INSERT INTO `elite_db`.`tbTrustedSenders` (`Name`) VALUES ('EDAPI Trade Dangerous Plugin');
INSERT INTO `elite_db`.`tbTrustedSenders` (`Name`) VALUES ('E:D Market Connector [Mac OS]');
INSERT INTO `elite_db`.`tbTrustedSenders` (`Name`) VALUES ('ED-IBE (API)');
INSERT INTO `elite_db`.`tbTrustedSenders` (`Name`) VALUES ('EVA [iPad]');
INSERT INTO `elite_db`.`tbTrustedSenders` (`Name`) VALUES ('EVA [iPhone]');
INSERT INTO `elite_db`.`tbTrustedSenders` (`Name`) VALUES ('EVA [Android]');
INSERT INTO `elite_db`.`tbTrustedSenders` (`Name`) VALUES ('EDDiscovery');
INSERT INTO `elite_db`.`tbTrustedSenders` (`Name`) VALUES ('EDDI');

COMMIT;


-- -----------------------------------------------------
-- Data for table `elite_db`.`tbDNMap_Commodity`
-- -----------------------------------------------------
START TRANSACTION;
USE `elite_db`;
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Advanceo Catalysers', '', 'Advanced Catalysers', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Agricultural Medicines', '', 'Agri-Medicines', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Ai Relics', '', 'AI Relics', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Animalmeat', '', 'Animal Meat', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Aovanced Medicines', '', 'Advanced Medicines', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Atmospheric Extractors', '', 'Atmospheric Processors', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Auto Fabricators', '', 'Auto-Fabricators', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Basic Narcotics', '', 'Narcotics', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Bio Reducing Lichen', '', 'Bioreducing Lichen', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('C M M Composite', '', 'CMM Composite', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Comercial Samples', '', 'Commercial Samples', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Diagnostic Sensor', '', 'Hardware Diagnostic Sensor', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Drones', '', 'Limpet', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Encripted Data Storage', '', 'Encrypted Data Storage', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('H N Shock Mount', '', 'HN Shock Mount', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Hafnium178', '', 'Hafnium 178', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Hazardous Environment Suits', '', 'H.E. Suits', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Heliostatic Furnaces', '', 'Microbial Furnaces', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Hr Shock Mount', '', 'HN Shock Mount', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Liquid Oxgen', '', 'Liquid Oxygen', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Marine Supplies', '', 'Marine Equipment', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Meta Alloys', '', 'Meta-Alloys', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Mu Tom Imager', '', 'Muon Imager', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Non Lethal Weapons', '', 'Non-Lethal Weapons', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Power Transfer Conduits', '', 'Power Transfer Bus', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('S A P8 Core Container', '', 'SAP 8 Core Container', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Skimer Components', '', 'Skimmer Components', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Terrain Enrichment Systems', '', 'Land Enrichment Systems', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Trinkets Of Fortune', '', 'Trinkets Of Hidden Fortune', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('U S S Cargo Ancient Artefact', '', 'Ancient Artefact', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('U S S Cargo Experimental Chemicals', '', 'Experimental Chemicals', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('U S S Cargo Military Plans', '', 'Military Plans', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('U S S Cargo Prototype Tech', '', 'Prototype Tech', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('U S S Cargo Rebel Transmissions', '', 'Rebel Transmissions', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('U S S Cargo Technical Blueprints', '', 'Technical Blueprints', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('U S S Cargo Trade Data', '', 'Trade Data', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Unknown Artifact', '', 'Unknown Artefact', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Unknown Artifact2', '', 'Unknown Artefact', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Wreckage Components', '', 'Salvageable Wreckage', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Methanol Monohydrate', '', 'Methanol Monohydrate Crystals', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Cooling Hoses', '', 'Micro-Weave Cooling Hoses', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Low Temperature Diamond', '', 'Low Temperature Diamonds', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Occupied Cryo Pod', '', 'Occupied CryoPod', '', NULL);
INSERT INTO `elite_db`.`tbDNMap_Commodity` (`CompanionName`, `CompanionAddition`, `GameName`, `GameAddition`, `ts`) VALUES ('Power Grid Assembly', '', 'Energy Grid Assembly', '', NULL);

COMMIT;


-- -----------------------------------------------------
-- Data for table `elite_db`.`tbEDDNRelays`
-- -----------------------------------------------------
START TRANSACTION;
USE `elite_db`;
INSERT INTO `elite_db`.`tbEDDNRelays` (`Address`) VALUES ('tcp://eddn-relay.elite-markets.net:9500');
INSERT INTO `elite_db`.`tbEDDNRelays` (`Address`) VALUES ('tcp://eddn.edcd.io:9500 ');

COMMIT;

