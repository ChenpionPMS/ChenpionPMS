CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(95) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
);

CREATE TABLE IF NOT EXISTS `__SeedMigrationsHistory` (
    `MigrationId` varchar(95) NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
);

CREATE TABLE `MenuItem` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` text NULL,
    `Title` text NULL,
    `Href` text NULL,
    `Icon` text NULL,
    `Sequence` int NOT NULL,
    `IsActive` bit NOT NULL,
    `MenuItemId` int NULL,
    CONSTRAINT `PK_MenuItem` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_MenuItem_MenuItem_MenuItemId` FOREIGN KEY (`MenuItemId`) REFERENCES `MenuItem` (`Id`) ON DELETE RESTRICT
);

CREATE TABLE `Role` (
    `Id` varchar(127) NOT NULL,
    `Name` varchar(127) NULL,
    `NormalizedName` varchar(127) NULL,
    `ConcurrencyStamp` text NULL,
    `Description` text NULL,
    `IsBuiltIn` bit NOT NULL,
    CONSTRAINT `PK_Role` PRIMARY KEY (`Id`)
);

CREATE TABLE `User` (
    `Id` varchar(127) NOT NULL,
    `UserName` varchar(127) NULL,
    `NormalizedUserName` varchar(127) NULL,
    `Email` varchar(127) NULL,
    `NormalizedEmail` varchar(127) NULL,
    `EmailConfirmed` bit NOT NULL,
    `PasswordHash` text NULL,
    `SecurityStamp` text NULL,
    `ConcurrencyStamp` text NULL,
    `PhoneNumber` text NULL,
    `PhoneNumberConfirmed` bit NOT NULL,
    `TwoFactorEnabled` bit NOT NULL,
    `LockoutEnd` datetime(6) NULL,
    `LockoutEnabled` bit NOT NULL,
    `AccessFailedCount` int NOT NULL,
    `Avatar` text NULL,
    `FirstName` text NULL,
    `LastName` text NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `UpdatedAt` datetime(6) NOT NULL,
    `DeletedAt` datetime(6) NULL,
    CONSTRAINT `PK_User` PRIMARY KEY (`Id`)
);

CREATE TABLE `RoleClaim` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `RoleId` varchar(127) NOT NULL,
    `ClaimType` text NULL,
    `ClaimValue` text NULL,
    CONSTRAINT `PK_RoleClaim` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_RoleClaim_Role_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Role` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `RoleMenuItem` (
    `RoleId` varchar(127) NOT NULL,
    `MenuItemId` int NOT NULL,
    `Visible` bit NOT NULL,
    `Permission` text NULL,
    `LockPermission` bit NOT NULL,
    CONSTRAINT `PK_RoleMenuItem` PRIMARY KEY (`RoleId`, `MenuItemId`),
    CONSTRAINT `FK_RoleMenuItem_MenuItem_MenuItemId` FOREIGN KEY (`MenuItemId`) REFERENCES `MenuItem` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_RoleMenuItem_Role_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Role` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `UserClaim` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` varchar(127) NOT NULL,
    `ClaimType` text NULL,
    `ClaimValue` text NULL,
    CONSTRAINT `PK_UserClaim` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_UserClaim_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `UserLogin` (
    `LoginProvider` varchar(127) NOT NULL,
    `ProviderKey` varchar(127) NOT NULL,
    `ProviderDisplayName` text NULL,
    `UserId` varchar(127) NOT NULL,
    CONSTRAINT `PK_UserLogin` PRIMARY KEY (`LoginProvider`, `ProviderKey`),
    CONSTRAINT `FK_UserLogin_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `UserRole` (
    `UserId` varchar(127) NOT NULL,
    `RoleId` varchar(127) NOT NULL,
    CONSTRAINT `PK_UserRole` PRIMARY KEY (`UserId`, `RoleId`),
    CONSTRAINT `FK_UserRole_Role_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Role` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_UserRole_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `UserToken` (
    `UserId` varchar(127) NOT NULL,
    `LoginProvider` varchar(127) NOT NULL,
    `Name` varchar(127) NOT NULL,
    `Value` text NULL,
    CONSTRAINT `PK_UserToken` PRIMARY KEY (`UserId`, `LoginProvider`, `Name`),
    CONSTRAINT `FK_UserToken_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`Id`) ON DELETE CASCADE
);

CREATE INDEX `IX_MenuItem_MenuItemId` ON `MenuItem` (`MenuItemId`);

CREATE UNIQUE INDEX `RoleNameIndex` ON `Role` (`NormalizedName`);

CREATE INDEX `IX_RoleClaim_RoleId` ON `RoleClaim` (`RoleId`);

CREATE INDEX `IX_RoleMenuItem_MenuItemId` ON `RoleMenuItem` (`MenuItemId`);

CREATE INDEX `EmailIndex` ON `User` (`NormalizedEmail`);

CREATE UNIQUE INDEX `UserNameIndex` ON `User` (`NormalizedUserName`);

CREATE INDEX `IX_UserClaim_UserId` ON `UserClaim` (`UserId`);

CREATE INDEX `IX_UserLogin_UserId` ON `UserLogin` (`UserId`);

CREATE INDEX `IX_UserRole_RoleId` ON `UserRole` (`RoleId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20190929163026_InitialCreate', '2.2.6-servicing-10079');

