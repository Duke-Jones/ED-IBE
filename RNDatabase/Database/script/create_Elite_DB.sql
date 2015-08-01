-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema Elite_DB
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema Elite_DB
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `Elite_DB` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci ;
USE `Elite_DB` ;

-- -----------------------------------------------------
-- Table `Elite_DB`.`tbAllegiance`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbAllegiance` (
  `id` INT NOT NULL,
  `allegiance` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Elite_DB`.`tbEconomy`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbEconomy` (
  `id` INT NOT NULL,
  `economy` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Elite_DB`.`tbGovernment`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbGovernment` (
  `id` INT NOT NULL,
  `government` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Elite_DB`.`tbState`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbState` (
  `id` INT NOT NULL,
  `state` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Elite_DB`.`tbSecurity`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbSecurity` (
  `id` INT NOT NULL,
  `security` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Elite_DB`.`tbSystems`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbSystems` (
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
  `is_changed` TINYINT(1) NOT NULL DEFAULT 0,
  `visited` TINYINT(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`),
  INDEX `fk_tbSystems_tbAllegiance1_idx` (`allegiance_id` ASC),
  INDEX `fk_tbSystems_tbEconomy1_idx` (`primary_economy_id` ASC),
  INDEX `fk_tbSystems_tbGovernment1_idx` (`government_id` ASC),
  INDEX `fk_tbSystems_tbState1_idx` (`state_id` ASC),
  INDEX `fk_tbSystems_tbSecurity1_idx` (`security_id` ASC),
  INDEX `idx_x` USING BTREE (`x` ASC),
  INDEX `idx_y` USING BTREE (`y` ASC),
  INDEX `idx_z` USING BTREE (`z` ASC),
  CONSTRAINT `fk_tbSystems_tbAllegiance1`
    FOREIGN KEY (`allegiance_id`)
    REFERENCES `Elite_DB`.`tbAllegiance` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbSystems_tbEconomy1`
    FOREIGN KEY (`primary_economy_id`)
    REFERENCES `Elite_DB`.`tbEconomy` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbSystems_tbGovernment1`
    FOREIGN KEY (`government_id`)
    REFERENCES `Elite_DB`.`tbGovernment` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbSystems_tbState1`
    FOREIGN KEY (`state_id`)
    REFERENCES `Elite_DB`.`tbState` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbSystems_tbSecurity1`
    FOREIGN KEY (`security_id`)
    REFERENCES `Elite_DB`.`tbSecurity` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Elite_DB`.`tbStationType`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbStationType` (
  `id` INT NOT NULL,
  `stationtype` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Elite_DB`.`tbStations`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbStations` (
  `id` INT NOT NULL,
  `stationname` VARCHAR(80) NOT NULL,
  `system_id` INT NOT NULL,
  `max_landing_pad_size` CHAR(1) NULL,
  `distance_to_star` INT NULL,
  `faction` VARCHAR(80) NULL,
  `government_id` INT NULL,
  `allegiance_id` INT NULL,
  `state_id` INT NULL,
  `stationtype_id` INT NULL,
  `has_blackmarket` TINYINT(1) NULL,
  `has_commodities` TINYINT(1) NULL,
  `has_refuel` TINYINT(1) NULL,
  `has_repair` TINYINT(1) NULL,
  `has_rearm` TINYINT(1) NULL,
  `has_outfitting` TINYINT(1) NULL,
  `updated_at` DATETIME NOT NULL,
  `is_changed` TINYINT(1) NOT NULL DEFAULT 0,
  `visited` TINYINT(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`),
  INDEX `fk_tbStations_tbSystems_idx` (`system_id` ASC),
  INDEX `fk_tbStations_tbAllegiance1_idx` (`allegiance_id` ASC),
  INDEX `fk_tbStations_tbGovernment1_idx` (`government_id` ASC),
  INDEX `fk_tbStations_tbState1_idx` (`state_id` ASC),
  INDEX `fk_tbStations_tbStationType1_idx` (`stationtype_id` ASC),
  CONSTRAINT `fk_tbStations_tbSystems`
    FOREIGN KEY (`system_id`)
    REFERENCES `Elite_DB`.`tbSystems` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbStations_tbAllegiance1`
    FOREIGN KEY (`allegiance_id`)
    REFERENCES `Elite_DB`.`tbAllegiance` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbStations_tbGovernment1`
    FOREIGN KEY (`government_id`)
    REFERENCES `Elite_DB`.`tbGovernment` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbStations_tbState1`
    FOREIGN KEY (`state_id`)
    REFERENCES `Elite_DB`.`tbState` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbStations_tbStationType1`
    FOREIGN KEY (`stationtype_id`)
    REFERENCES `Elite_DB`.`tbStationType` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Elite_DB`.`tbCategory`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbCategory` (
  `id` INT NOT NULL,
  `category` VARCHAR(80) NOT NULL,
  `loccategory` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Elite_DB`.`tbCommodity`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbCommodity` (
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
    REFERENCES `Elite_DB`.`tbCategory` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Elite_DB`.`tbImportCommodity`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbImportCommodity` (
  `station_id` INT NOT NULL,
  `commodity_id` INT NOT NULL,
  PRIMARY KEY (`station_id`, `commodity_id`),
  INDEX `fk_tbStations_has_tbCommodities_tbCommodities1_idx` (`commodity_id` ASC),
  INDEX `fk_tbStations_has_tbCommodities_tbStations1_idx` (`station_id` ASC),
  CONSTRAINT `fk_tbStations_has_tbCommodities_tbStations1`
    FOREIGN KEY (`station_id`)
    REFERENCES `Elite_DB`.`tbStations` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbStations_has_tbCommodities_tbCommodities1`
    FOREIGN KEY (`commodity_id`)
    REFERENCES `Elite_DB`.`tbCommodity` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Elite_DB`.`tbExportCommodity`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbExportCommodity` (
  `station_id` INT NOT NULL,
  `commodity_id` INT NOT NULL,
  PRIMARY KEY (`station_id`, `commodity_id`),
  INDEX `fk_tbStations_has_tbCommodities_tbCommodities1_idx` (`commodity_id` ASC),
  INDEX `fk_tbStations_has_tbCommodities_tbStations1_idx` (`station_id` ASC),
  CONSTRAINT `fk_tbStations_has_tbCommodities_tbStations10`
    FOREIGN KEY (`station_id`)
    REFERENCES `Elite_DB`.`tbStations` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbStations_has_tbCommodities_tbCommodities10`
    FOREIGN KEY (`commodity_id`)
    REFERENCES `Elite_DB`.`tbCommodity` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Elite_DB`.`tbProhibitedCommodity`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbProhibitedCommodity` (
  `station_id` INT NOT NULL,
  `commodity_id` INT NOT NULL,
  PRIMARY KEY (`station_id`, `commodity_id`),
  INDEX `fk_tbStations_has_tbCommodities_tbCommodities1_idx` (`commodity_id` ASC),
  INDEX `fk_tbStations_has_tbCommodities_tbStations1_idx` (`station_id` ASC),
  CONSTRAINT `fk_tbStations_has_tbCommodities_tbStations100`
    FOREIGN KEY (`station_id`)
    REFERENCES `Elite_DB`.`tbStations` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbStations_has_tbCommodities_tbCommodities100`
    FOREIGN KEY (`commodity_id`)
    REFERENCES `Elite_DB`.`tbCommodity` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Elite_DB`.`tbSources`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbSources` (
  `id` INT NOT NULL,
  `sourcename` VARCHAR(80) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Elite_DB`.`tbPrice`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbPrice` (
  `station_id` INT NOT NULL,
  `commodity_id` INT NOT NULL,
  `price` INT NOT NULL,
  `timestamp` DATETIME NOT NULL,
  `source_id` INT NOT NULL,
  PRIMARY KEY (`station_id`, `commodity_id`),
  INDEX `fk_tbStations_has_tbCommodities_tbCommodities2_idx` (`commodity_id` ASC),
  INDEX `fk_tbStations_has_tbCommodities_tbStations2_idx` (`station_id` ASC),
  INDEX `fk_tbPrice_tbSources1_idx` (`source_id` ASC),
  CONSTRAINT `fk_tbStations_has_tbCommodities_tbStations2`
    FOREIGN KEY (`station_id`)
    REFERENCES `Elite_DB`.`tbStations` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbStations_has_tbCommodities_tbCommodities2`
    FOREIGN KEY (`commodity_id`)
    REFERENCES `Elite_DB`.`tbCommodity` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbPrice_tbSources1`
    FOREIGN KEY (`source_id`)
    REFERENCES `Elite_DB`.`tbSources` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Elite_DB`.`tbPriceHistory`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbPriceHistory` (
  `tbStations_id` INT NOT NULL,
  `tbCommodities_id` INT NOT NULL,
  `price` INT NOT NULL,
  `timestamp` DATETIME NOT NULL,
  `source_id` INT NOT NULL,
  PRIMARY KEY (`tbStations_id`, `tbCommodities_id`),
  INDEX `fk_tbStations_has_tbCommodities1_tbCommodities1_idx` (`tbCommodities_id` ASC),
  INDEX `fk_tbStations_has_tbCommodities1_tbStations1_idx` (`tbStations_id` ASC),
  INDEX `fk_tbPriceHistory_tbSources1_idx` (`source_id` ASC),
  CONSTRAINT `fk_tbStations_has_tbCommodities1_tbStations1`
    FOREIGN KEY (`tbStations_id`)
    REFERENCES `Elite_DB`.`tbStations` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbStations_has_tbCommodities1_tbCommodities1`
    FOREIGN KEY (`tbCommodities_id`)
    REFERENCES `Elite_DB`.`tbCommodity` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbPriceHistory_tbSources1`
    FOREIGN KEY (`source_id`)
    REFERENCES `Elite_DB`.`tbSources` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Elite_DB`.`tbStationEconomy`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbStationEconomy` (
  `station_id` INT NOT NULL,
  `economy_id` INT NOT NULL,
  PRIMARY KEY (`station_id`, `economy_id`),
  INDEX `fk_tbStations_has_tbEconomy_tbEconomy1_idx` (`economy_id` ASC),
  INDEX `fk_tbStations_has_tbEconomy_tbStations1_idx` (`station_id` ASC),
  CONSTRAINT `fk_tbStations_has_tbEconomy_tbStations1`
    FOREIGN KEY (`station_id`)
    REFERENCES `Elite_DB`.`tbStations` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbStations_has_tbEconomy_tbEconomy1`
    FOREIGN KEY (`economy_id`)
    REFERENCES `Elite_DB`.`tbEconomy` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Elite_DB`.`tbLanguage`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbLanguage` (
  `id` INT NOT NULL,
  `language` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Elite_DB`.`tbCommodityLocalization`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbCommodityLocalization` (
  `commodity_id` INT NOT NULL,
  `language_id` INT NOT NULL,
  `locname` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`commodity_id`, `language_id`),
  INDEX `fk_tbLocalization_tbLanguage1_idx` (`language_id` ASC),
  CONSTRAINT `fk_tbLocalization_tbCommodities1`
    FOREIGN KEY (`commodity_id`)
    REFERENCES `Elite_DB`.`tbCommodity` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbLocalization_tbLanguage1`
    FOREIGN KEY (`language_id`)
    REFERENCES `Elite_DB`.`tbLanguage` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Elite_DB`.`tbCategoryLocalization`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbCategoryLocalization` (
  `category_id` INT NOT NULL,
  `language_id` INT NOT NULL,
  `locname` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`category_id`, `language_id`),
  INDEX `fk_tbLocalization_tbLanguage1_idx` (`language_id` ASC),
  CONSTRAINT `fk_tbLocalization_tbLanguage10`
    FOREIGN KEY (`language_id`)
    REFERENCES `Elite_DB`.`tbLanguage` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbCategoryLocalization_tbCategory1`
    FOREIGN KEY (`category_id`)
    REFERENCES `Elite_DB`.`tbCategory` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Elite_DB`.`tbInitValue`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbInitValue` (
  `InitGroup` VARCHAR(80) NOT NULL,
  `InitKey` VARCHAR(80) NOT NULL,
  `InitValue` VARCHAR(255) NULL,
  PRIMARY KEY (`InitGroup`, `InitKey`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Elite_DB`.`tbEventType`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbEventType` (
  `id` INT NOT NULL,
  `event` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Elite_DB`.`tbCargoAction`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbCargoAction` (
  `id` INT NOT NULL,
  `action` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Elite_DB`.`tbLog`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbLog` (
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
  INDEX `fk_tbLog_tbCommodities1_idx` (`commodity_id` ASC),
  INDEX `fk_tbLog_tbSystems1_idx` (`system_id` ASC),
  INDEX `fk_tbLog_tbStations1_idx` (`station_id` ASC),
  INDEX `fk_tbLog_tbEventType1_idx` (`event_id` ASC),
  INDEX `fk_tbLog_tbCargoAction1_idx` (`cargoaction_id` ASC),
  PRIMARY KEY (`time`),
  CONSTRAINT `fk_tbLog_tbCommodities1`
    FOREIGN KEY (`commodity_id`)
    REFERENCES `Elite_DB`.`tbCommodity` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbLog_tbSystems1`
    FOREIGN KEY (`system_id`)
    REFERENCES `Elite_DB`.`tbSystems` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbLog_tbStations1`
    FOREIGN KEY (`station_id`)
    REFERENCES `Elite_DB`.`tbStations` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE,
  CONSTRAINT `fk_tbLog_tbEventType1`
    FOREIGN KEY (`event_id`)
    REFERENCES `Elite_DB`.`tbEventType` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbLog_tbCargoAction1`
    FOREIGN KEY (`cargoaction_id`)
    REFERENCES `Elite_DB`.`tbCargoAction` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Elite_DB`.`tbSystems_org`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbSystems_org` (
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
  `is_changed` TINYINT(1) NOT NULL DEFAULT 0,
  `visited` TINYINT(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`),
  INDEX `fk_tbSystems_tbAllegiance1_idx` (`allegiance_id` ASC),
  INDEX `fk_tbSystems_tbEconomy1_idx` (`primary_economy_id` ASC),
  INDEX `fk_tbSystems_tbGovernment1_idx` (`government_id` ASC),
  INDEX `fk_tbSystems_tbState1_idx` (`state_id` ASC),
  INDEX `fk_tbSystems_tbSecurity1_idx` (`security_id` ASC),
  INDEX `idx_x` USING BTREE (`x` ASC),
  INDEX `idx_y` USING BTREE (`y` ASC),
  INDEX `idx_z` USING BTREE (`z` ASC),
  CONSTRAINT `fk_tbSystems_tbAllegiance10`
    FOREIGN KEY (`allegiance_id`)
    REFERENCES `Elite_DB`.`tbAllegiance` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbSystems_tbEconomy10`
    FOREIGN KEY (`primary_economy_id`)
    REFERENCES `Elite_DB`.`tbEconomy` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbSystems_tbGovernment10`
    FOREIGN KEY (`government_id`)
    REFERENCES `Elite_DB`.`tbGovernment` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbSystems_tbState10`
    FOREIGN KEY (`state_id`)
    REFERENCES `Elite_DB`.`tbState` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbSystems_tbSecurity10`
    FOREIGN KEY (`security_id`)
    REFERENCES `Elite_DB`.`tbSecurity` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Elite_DB`.`tbStations_org`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbStations_org` (
  `id` INT NOT NULL,
  `stationname` VARCHAR(80) NOT NULL,
  `system_id` INT NOT NULL,
  `max_landing_pad_size` CHAR(1) NULL,
  `distance_to_star` INT NULL,
  `faction` VARCHAR(80) NULL,
  `government_id` INT NULL,
  `allegiance_id` INT NULL,
  `state_id` INT NULL,
  `stationtype_id` INT NULL,
  `has_blackmarket` TINYINT(1) NULL,
  `has_commodities` TINYINT(1) NULL,
  `has_refuel` TINYINT(1) NULL,
  `has_repair` TINYINT(1) NULL,
  `has_rearm` TINYINT(1) NULL,
  `has_outfitting` TINYINT(1) NULL,
  `updated_at` DATETIME NOT NULL,
  `is_changed` TINYINT(1) NOT NULL DEFAULT 0,
  `visited` TINYINT(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`),
  INDEX `fk_tbStations_tbSystems_idx` (`system_id` ASC),
  INDEX `fk_tbStations_tbAllegiance1_idx` (`allegiance_id` ASC),
  INDEX `fk_tbStations_tbGovernment1_idx` (`government_id` ASC),
  INDEX `fk_tbStations_tbState1_idx` (`state_id` ASC),
  INDEX `fk_tbStations_tbStationType1_idx` (`stationtype_id` ASC),
  CONSTRAINT `fk_tbStations_tbSystems0`
    FOREIGN KEY (`system_id`)
    REFERENCES `Elite_DB`.`tbSystems` (`id`)
    ON DELETE CASCADE
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbStations_tbAllegiance10`
    FOREIGN KEY (`allegiance_id`)
    REFERENCES `Elite_DB`.`tbAllegiance` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbStations_tbGovernment10`
    FOREIGN KEY (`government_id`)
    REFERENCES `Elite_DB`.`tbGovernment` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbStations_tbState10`
    FOREIGN KEY (`state_id`)
    REFERENCES `Elite_DB`.`tbState` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_tbStations_tbStationType10`
    FOREIGN KEY (`stationtype_id`)
    REFERENCES `Elite_DB`.`tbStationType` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Elite_DB`.`tbVisitedSystems`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbVisitedSystems` (
  `system_id` INT NOT NULL,
  `time` DATETIME NOT NULL,
  PRIMARY KEY (`system_id`),
  CONSTRAINT `fk_tbSystems_tbVisitedSystems`
    FOREIGN KEY (`system_id`)
    REFERENCES `Elite_DB`.`tbSystems` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Elite_DB`.`tbVisitedStations`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Elite_DB`.`tbVisitedStations` (
  `station_id` INT NOT NULL,
  `time` DATETIME NOT NULL,
  PRIMARY KEY (`station_id`),
  CONSTRAINT `fk_tbStations_tbVisitedStations`
    FOREIGN KEY (`station_id`)
    REFERENCES `Elite_DB`.`tbStations` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

-- -----------------------------------------------------
-- Data for table `Elite_DB`.`tbAllegiance`
-- -----------------------------------------------------
START TRANSACTION;
USE `Elite_DB`;
INSERT INTO `Elite_DB`.`tbAllegiance` (`id`, `allegiance`) VALUES (0, 'Alliance');
INSERT INTO `Elite_DB`.`tbAllegiance` (`id`, `allegiance`) VALUES (1, 'Anarchy');
INSERT INTO `Elite_DB`.`tbAllegiance` (`id`, `allegiance`) VALUES (2, 'Empire');
INSERT INTO `Elite_DB`.`tbAllegiance` (`id`, `allegiance`) VALUES (3, 'Federation');
INSERT INTO `Elite_DB`.`tbAllegiance` (`id`, `allegiance`) VALUES (4, 'Independent');
INSERT INTO `Elite_DB`.`tbAllegiance` (`id`, `allegiance`) VALUES (5, 'None');

COMMIT;


-- -----------------------------------------------------
-- Data for table `Elite_DB`.`tbEconomy`
-- -----------------------------------------------------
START TRANSACTION;
USE `Elite_DB`;
INSERT INTO `Elite_DB`.`tbEconomy` (`id`, `economy`) VALUES (0, 'Agriculture');
INSERT INTO `Elite_DB`.`tbEconomy` (`id`, `economy`) VALUES (1, 'Extraction');
INSERT INTO `Elite_DB`.`tbEconomy` (`id`, `economy`) VALUES (2, 'High Tech');
INSERT INTO `Elite_DB`.`tbEconomy` (`id`, `economy`) VALUES (3, 'Industrial');
INSERT INTO `Elite_DB`.`tbEconomy` (`id`, `economy`) VALUES (4, 'Military');
INSERT INTO `Elite_DB`.`tbEconomy` (`id`, `economy`) VALUES (5, 'Refinery');
INSERT INTO `Elite_DB`.`tbEconomy` (`id`, `economy`) VALUES (6, 'Service');
INSERT INTO `Elite_DB`.`tbEconomy` (`id`, `economy`) VALUES (7, 'Terraforming');
INSERT INTO `Elite_DB`.`tbEconomy` (`id`, `economy`) VALUES (8, 'Tourism');
INSERT INTO `Elite_DB`.`tbEconomy` (`id`, `economy`) VALUES (9, 'None');

COMMIT;


-- -----------------------------------------------------
-- Data for table `Elite_DB`.`tbGovernment`
-- -----------------------------------------------------
START TRANSACTION;
USE `Elite_DB`;
INSERT INTO `Elite_DB`.`tbGovernment` (`id`, `government`) VALUES (0, 'None');
INSERT INTO `Elite_DB`.`tbGovernment` (`id`, `government`) VALUES (1, 'Anarchy');
INSERT INTO `Elite_DB`.`tbGovernment` (`id`, `government`) VALUES (2, 'Communism');
INSERT INTO `Elite_DB`.`tbGovernment` (`id`, `government`) VALUES (3, 'Confederacy');
INSERT INTO `Elite_DB`.`tbGovernment` (`id`, `government`) VALUES (4, 'Corporate');
INSERT INTO `Elite_DB`.`tbGovernment` (`id`, `government`) VALUES (5, 'Cooperative');
INSERT INTO `Elite_DB`.`tbGovernment` (`id`, `government`) VALUES (6, 'Democracy');
INSERT INTO `Elite_DB`.`tbGovernment` (`id`, `government`) VALUES (7, 'Dictatorship');
INSERT INTO `Elite_DB`.`tbGovernment` (`id`, `government`) VALUES (8, 'Feudal');
INSERT INTO `Elite_DB`.`tbGovernment` (`id`, `government`) VALUES (9, 'Imperial');
INSERT INTO `Elite_DB`.`tbGovernment` (`id`, `government`) VALUES (10, 'Patronage');
INSERT INTO `Elite_DB`.`tbGovernment` (`id`, `government`) VALUES (11, 'Colony');
INSERT INTO `Elite_DB`.`tbGovernment` (`id`, `government`) VALUES (12, 'Prison Colony');
INSERT INTO `Elite_DB`.`tbGovernment` (`id`, `government`) VALUES (13, 'Theocracy');

COMMIT;


-- -----------------------------------------------------
-- Data for table `Elite_DB`.`tbState`
-- -----------------------------------------------------
START TRANSACTION;
USE `Elite_DB`;
INSERT INTO `Elite_DB`.`tbState` (`id`, `state`) VALUES (0, 'Boom');
INSERT INTO `Elite_DB`.`tbState` (`id`, `state`) VALUES (1, 'Bust');
INSERT INTO `Elite_DB`.`tbState` (`id`, `state`) VALUES (2, 'Civil Unrest');
INSERT INTO `Elite_DB`.`tbState` (`id`, `state`) VALUES (3, 'Civil War');
INSERT INTO `Elite_DB`.`tbState` (`id`, `state`) VALUES (4, 'Expansion');
INSERT INTO `Elite_DB`.`tbState` (`id`, `state`) VALUES (5, 'Lockdown');
INSERT INTO `Elite_DB`.`tbState` (`id`, `state`) VALUES (6, 'Outbreak');
INSERT INTO `Elite_DB`.`tbState` (`id`, `state`) VALUES (7, 'War');
INSERT INTO `Elite_DB`.`tbState` (`id`, `state`) VALUES (8, 'None');

COMMIT;


-- -----------------------------------------------------
-- Data for table `Elite_DB`.`tbSecurity`
-- -----------------------------------------------------
START TRANSACTION;
USE `Elite_DB`;
INSERT INTO `Elite_DB`.`tbSecurity` (`id`, `security`) VALUES (0, 'Low');
INSERT INTO `Elite_DB`.`tbSecurity` (`id`, `security`) VALUES (1, 'Medium');
INSERT INTO `Elite_DB`.`tbSecurity` (`id`, `security`) VALUES (2, 'High');

COMMIT;


-- -----------------------------------------------------
-- Data for table `Elite_DB`.`tbStationType`
-- -----------------------------------------------------
START TRANSACTION;
USE `Elite_DB`;
INSERT INTO `Elite_DB`.`tbStationType` (`id`, `stationtype`) VALUES (0, 'Civilian Outpost');
INSERT INTO `Elite_DB`.`tbStationType` (`id`, `stationtype`) VALUES (1, 'Commercial Outpost');
INSERT INTO `Elite_DB`.`tbStationType` (`id`, `stationtype`) VALUES (2, 'Coriolis Starport');
INSERT INTO `Elite_DB`.`tbStationType` (`id`, `stationtype`) VALUES (3, 'Industrial Outpost');
INSERT INTO `Elite_DB`.`tbStationType` (`id`, `stationtype`) VALUES (4, 'Military Outpost');
INSERT INTO `Elite_DB`.`tbStationType` (`id`, `stationtype`) VALUES (5, 'Mining Outpost');
INSERT INTO `Elite_DB`.`tbStationType` (`id`, `stationtype`) VALUES (6, 'Ocellus Starport');
INSERT INTO `Elite_DB`.`tbStationType` (`id`, `stationtype`) VALUES (7, 'Orbis Starport');
INSERT INTO `Elite_DB`.`tbStationType` (`id`, `stationtype`) VALUES (8, 'Scientific Outpost');
INSERT INTO `Elite_DB`.`tbStationType` (`id`, `stationtype`) VALUES (9, 'Unsanctioned Outpost');
INSERT INTO `Elite_DB`.`tbStationType` (`id`, `stationtype`) VALUES (10, 'Unknown Outpost');
INSERT INTO `Elite_DB`.`tbStationType` (`id`, `stationtype`) VALUES (11, 'Unknown Starport');

COMMIT;


-- -----------------------------------------------------
-- Data for table `Elite_DB`.`tbLanguage`
-- -----------------------------------------------------
START TRANSACTION;
USE `Elite_DB`;
INSERT INTO `Elite_DB`.`tbLanguage` (`id`, `language`) VALUES (0, 'eng');
INSERT INTO `Elite_DB`.`tbLanguage` (`id`, `language`) VALUES (1, 'ger');
INSERT INTO `Elite_DB`.`tbLanguage` (`id`, `language`) VALUES (2, 'fra');

COMMIT;


-- -----------------------------------------------------
-- Data for table `Elite_DB`.`tbInitValue`
-- -----------------------------------------------------
START TRANSACTION;
USE `Elite_DB`;
INSERT INTO `Elite_DB`.`tbInitValue` (`InitGroup`, `InitKey`, `InitValue`) VALUES ('DB', 'StructureRev', '1');

COMMIT;


-- -----------------------------------------------------
-- Data for table `Elite_DB`.`tbEventType`
-- -----------------------------------------------------
START TRANSACTION;
USE `Elite_DB`;
INSERT INTO `Elite_DB`.`tbEventType` (`id`, `event`) VALUES (1, 'Jumped To');
INSERT INTO `Elite_DB`.`tbEventType` (`id`, `event`) VALUES (2, 'Visited');
INSERT INTO `Elite_DB`.`tbEventType` (`id`, `event`) VALUES (3, 'Market Data Collected');
INSERT INTO `Elite_DB`.`tbEventType` (`id`, `event`) VALUES (4, 'Cargo');
INSERT INTO `Elite_DB`.`tbEventType` (`id`, `event`) VALUES (5, 'Fight');
INSERT INTO `Elite_DB`.`tbEventType` (`id`, `event`) VALUES (6, 'Docked');
INSERT INTO `Elite_DB`.`tbEventType` (`id`, `event`) VALUES (7, 'Took Off');
INSERT INTO `Elite_DB`.`tbEventType` (`id`, `event`) VALUES (8, 'Saved Game');
INSERT INTO `Elite_DB`.`tbEventType` (`id`, `event`) VALUES (9, 'Loaded Game');
INSERT INTO `Elite_DB`.`tbEventType` (`id`, `event`) VALUES (10, 'Accepted Mission');
INSERT INTO `Elite_DB`.`tbEventType` (`id`, `event`) VALUES (11, 'Completed Mission');
INSERT INTO `Elite_DB`.`tbEventType` (`id`, `event`) VALUES (12, 'Other');

COMMIT;


-- -----------------------------------------------------
-- Data for table `Elite_DB`.`tbCargoAction`
-- -----------------------------------------------------
START TRANSACTION;
USE `Elite_DB`;
INSERT INTO `Elite_DB`.`tbCargoAction` (`id`, `action`) VALUES (1, 'Bought');
INSERT INTO `Elite_DB`.`tbCargoAction` (`id`, `action`) VALUES (2, 'Sold');
INSERT INTO `Elite_DB`.`tbCargoAction` (`id`, `action`) VALUES (3, 'Mined');
INSERT INTO `Elite_DB`.`tbCargoAction` (`id`, `action`) VALUES (4, 'Stolen');
INSERT INTO `Elite_DB`.`tbCargoAction` (`id`, `action`) VALUES (5, 'Found');

COMMIT;

