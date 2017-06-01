namespace Cake.KeePass.Test.Integration
{
    using System;

    using Cake.Core.IO;

    using Xunit;

    public class KeePassReaderFixture
    {
        private const string IntegrationTestDatabasePassword = "Pass@word!";

        [Fact]
        public void Execute_DatabaseSettingsNull_ThrowsArgumentNullException()
        {
            // setup
            var entryCriteria = new KeePassEntryCriteria
                                    {
                                        GroupHierarchy = new[] { "One", "Two", "Three" },
                                        Title = "Test Entry"
                                    };

            // exercise
            var actual = Assert.Throws<ArgumentNullException>(() => KeePassReader.Execute(null, null, entryCriteria));

            // verify
            Assert.Equal("databaseSettings", actual.ParamName);
        }

        [Fact]
        public void Execute_EntryCriteriaNull_ThrowsArgumentNullException()
        {
            // setup
            var dbSettings = new KeePassDatabaseSettings()
                                 {
                                     DatabasePath = FilePath.FromString(@"IntegrationTestDatabase.kdbx"),
                                     MasterPassword = "Wrong-Password"
                                 };


            // exercise
            var actual = Assert.Throws<ArgumentNullException>(() => KeePassReader.Execute(null, dbSettings, null));

            // verify
            Assert.Equal("entryCriteria", actual.ParamName);
        }

        [Fact]
        public void Execute_MasterPasswordWrong_ThrowsException()
        {
            // setup
            var dbSettings = new KeePassDatabaseSettings()
                                 {
                                     DatabasePath = FilePath.FromString(@"IntegrationTestDatabase.kdbx"),
                                     MasterPassword = "Wrong-Password"
                                 };

            var entryCriteria = new KeePassEntryCriteria
                                    {
                                        GroupHierarchy = new[] { "One", "Two", "Three" },
                                        Title = "Test Entry"
                                    };

            // exercise
            var actual = Assert.Throws<Exception>(() => KeePassReader.Execute(null, dbSettings, entryCriteria));

            // verify
            Assert.Equal("Composite Key (password) provided was invalid.", actual.Message);
        }

        [Fact]
        public void Execute_NotAKeePassDatabase_ThrowsException()
        {
            // setup
            var dbSettings = new KeePassDatabaseSettings()
                                 {
                                     DatabasePath = FilePath.FromString(@"Cake.Core.dll"),
                                     MasterPassword = "Wrong-Password"
                                 };

            var entryCriteria = new KeePassEntryCriteria
                                    {
                                        GroupHierarchy = new[] { "One", "Two", "Three" },
                                        Title = "Test Entry"
                                    };

            // exercise
            var actual = Assert.Throws<Exception>(() => KeePassReader.Execute(null, dbSettings, entryCriteria));

            // verify 
            Assert.Equal("Invalid database file.", actual.Message);
        }

        [Fact]
        public void Execute_NoHierarchy_LoadsEntry_UsernameCorrect()
        {
            // setup
            var dbSettings = new KeePassDatabaseSettings()
                                 {
                                     DatabasePath = FilePath.FromString(@"IntegrationTestDatabase.kdbx"),
                                     MasterPassword = IntegrationTestDatabasePassword
            };

            var entryCriteria = new KeePassEntryCriteria
                                    {
                                        Title = "Sample Entry #2"
                                    };

            // exercise
            var actual = KeePassReader.Execute(null, dbSettings, entryCriteria);

            // verify 
            Assert.Equal("Michael321", actual.Username);
        }

        [Fact]
        public void Execute_NoHierarchy_LoadsEntry_PasswordCorrect()
        {
            // setup
            var dbSettings = new KeePassDatabaseSettings()
                                 {
                                     DatabasePath = FilePath.FromString(@"IntegrationTestDatabase.kdbx"),
                                     MasterPassword = IntegrationTestDatabasePassword
                                 };

            var entryCriteria = new KeePassEntryCriteria
                                    {
                                        Title = "Sample Entry #2"
                                    };

            // exercise
            var actual = KeePassReader.Execute(null, dbSettings, entryCriteria);

            // verify 
            Assert.Equal("12345", actual.Password);
        }

        [Fact]
        public void Execute_NoHierarchy_LoadsEntry_UrlCorrect()
        {
            // setup
            var dbSettings = new KeePassDatabaseSettings()
                                 {
                                     DatabasePath = FilePath.FromString(@"IntegrationTestDatabase.kdbx"),
                                     MasterPassword = IntegrationTestDatabasePassword
                                 };

            var entryCriteria = new KeePassEntryCriteria
                                    {
                                        Title = "Sample Entry #2"
                                    };

            // exercise
            var actual = KeePassReader.Execute(null, dbSettings, entryCriteria);

            // verify 
            Assert.Equal("http://keepass.info/help/kb/testform.html", actual.Url);
        }

        [Fact]
        public void Execute_NoHierarchy_LoadsEntry_UUidCorrect()
        {
            // setup
            var dbSettings = new KeePassDatabaseSettings()
                                 {
                                     DatabasePath = FilePath.FromString(@"IntegrationTestDatabase.kdbx"),
                                     MasterPassword = IntegrationTestDatabasePassword
                                 };

            var entryCriteria = new KeePassEntryCriteria
                                    {
                                        Title = "Sample Entry #2"
                                    };

            // exercise
            var actual = KeePassReader.Execute(null, dbSettings, entryCriteria);

            // verify 
            Assert.Equal("5765B5CF39794642A9283A2A9ECFC1CA", actual.Uuid);
        }

        [Fact]
        public void Execute_OneLevelHierarchy_LoadsEntry_UUidCorrect()
        {
            // setup
            var dbSettings = new KeePassDatabaseSettings()
                                 {
                                     DatabasePath = FilePath.FromString(@"IntegrationTestDatabase.kdbx"),
                                     MasterPassword = IntegrationTestDatabasePassword
                                 };

            var entryCriteria = new KeePassEntryCriteria
                                    {
                                        GroupHierarchy = new []{ "One" },
                                        Title = "One-Entry"
                                    };

            // exercise
            var actual = KeePassReader.Execute(null, dbSettings, entryCriteria);

            // verify 
            Assert.Equal("E866163BB5C33A42B177F9AF651D200F", actual.Uuid);
        }

        [Fact]
        public void Execute_TwoLevelHierarchy_LoadsEntry_UUidCorrect()
        {
            // setup
            var dbSettings = new KeePassDatabaseSettings()
                                 {
                                     DatabasePath = FilePath.FromString(@"IntegrationTestDatabase.kdbx"),
                                     MasterPassword = IntegrationTestDatabasePassword
                                 };

            var entryCriteria = new KeePassEntryCriteria
                                    {
                                        GroupHierarchy = new[] { "One", "Two" },
                                        Title = "Two-Entry"
                                    };

            // exercise
            var actual = KeePassReader.Execute(null, dbSettings, entryCriteria);

            // verify 
            Assert.Equal("5052C4736386E54E82F7BFBB34E5757F", actual.Uuid);
        }

        [Fact]
        public void Execute_NoHierarchy_LoadsEntryViaUUid_UUidCorrect()
        {
            // setup
            var dbSettings = new KeePassDatabaseSettings()
                                 {
                                     DatabasePath = FilePath.FromString(@"IntegrationTestDatabase.kdbx"),
                                     MasterPassword = IntegrationTestDatabasePassword
                                 };

            var entryCriteria = new KeePassEntryCriteria
                                    {
                                        Uuid = "5765B5CF39794642A9283A2A9ECFC1CA"
                                    };

            // exercise
            var actual = KeePassReader.Execute(null, dbSettings, entryCriteria);

            // verify 
            Assert.Equal("5765B5CF39794642A9283A2A9ECFC1CA", actual.Uuid);
        }
    }
}
