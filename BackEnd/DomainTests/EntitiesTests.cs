using Domain.Features.Address.Entities;
using Domain.Features.Address.Exceptions.Entities;
using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Features.Branch.Entities;
using Domain.Features.BranchOffer.Entities;
using Domain.Features.Characteristic.Entities;
using Domain.Features.Characteristic.Exceptions;
using Domain.Features.Characteristic.ValueObjects.Identificators;
using Domain.Features.Comment.Exceptions.ValueObjects;
using Domain.Features.Notification.Exceptions;
using Domain.Features.Offer.Exceptions.Entities;
using Domain.Features.Offer.Exceptions.ValueObjects;
using Domain.Features.Url.Entities;
using Domain.Features.Url.Exceptions.Entities;
using Domain.Features.Url.Exceptions.ValueObjects;
using Domain.Shared.Factories;
using Domain.Shared.Providers;
using Domain.Shared.Providers.ExceptionMessage;
using DomainTests.Fakes;
using Xunit.Abstractions;

namespace DomainTests
{
    public class EntitiesTests
    {
        //Properties
        private readonly ITestOutputHelper _output;
        private readonly IDomainFactory _domainFactory;


        //Constructor
        public EntitiesTests(ITestOutputHelper output)
        {
            _output = output;
            _domainFactory = new DomainFactory(
                new Provider(
                    new ExceptionMessageProvider(),
                    new Domain.Shared.Providers.Time.TimeProvider()),
                new DomainNotificationsFake(),
                new DomainUrlFake(),
                new CharacteristicsFake(),
                new CommentTypeFake());
        }


        //====================================================================================
        //====================================================================================
        //====================================================================================
        //Tests

        [InlineData(null)]
        [InlineData(1)]
        [InlineData(2)]
        [Theory]
        public void DomainNotification_Annul_Correct(int? notifaicationStstus)
        {
            var arrange = _domainFactory.CreateDomainNotification(
                Guid.NewGuid(),
                null,
                null,
                null,
                "",
                1,
                notifaicationStstus);
            arrange.Annul(); //Act
            _output.WriteLine($"{arrange.NotificationStatus.Id}");
            Assert.Equal(4, arrange.NotificationStatus.Id);
            Assert.Equal(true, arrange.IsReadedAnswerByUser.Value);
        }

        [InlineData(3)]
        [InlineData(4)]
        [Theory]
        public void DomainNotification_Annul_NotificationException(int notifaicationStstus)
        {
            var arrange = _domainFactory.CreateDomainNotification(
                null,
                "a@gmail.com",
                null,
                null,
                "",
                1,
                notifaicationStstus);
            Assert.Throws<NotificationException>(() => arrange.Annul());
        }

        [InlineData(1)]
        [InlineData(3)]
        [InlineData(4)]
        [Theory]
        public void DomainNotification_SetReadedByUser_NotificationException_HaveNoAnswer(int notifaicationStstus)
        {
            var arrange = _domainFactory.CreateDomainNotification(
                null,
                "a@gmail.com",
                null,
                null,
                "",
                1,
                notifaicationStstus);
            Assert.Throws<NotificationException>(() => arrange.SetReadedByUser());
        }
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(4)]
        [Theory]
        public void DomainNotification_SetReadedByUser_NotificationException_HasReadedByUser(int notifaicationStstus)
        {
            var arrange = _domainFactory.CreateDomainNotification(
                Guid.NewGuid(),
                Guid.NewGuid(),
                null,
                DateTime.Now.AddDays(-1),
                DateTime.Now.AddHours(-1),
                null,
                null,
                "aa",
                " aa ",
                "y",
                1,
                notifaicationStstus);
            Assert.Throws<NotificationException>(() => arrange.SetReadedByUser());
        }

        [InlineData(2)]
        [InlineData(1)]
        [Theory]
        public void DomainNotification_SetReadedByUser_Success(int notifaicationStstus)
        {
            var arrange = _domainFactory.CreateDomainNotification(
                Guid.NewGuid(),
                Guid.NewGuid(),
                null,
                DateTime.Now.AddDays(-1),
                DateTime.Now.AddHours(-1),
                null,
                null,
                "aa",
                " aa ",
                "n",
                1,
                notifaicationStstus);
            _output.WriteLine($"A{arrange.IsExistAnswer} R{arrange.IsReadedAnswerByUser.Value}");
            arrange.SetReadedByUser();
            _output.WriteLine($"A{arrange.IsExistAnswer} R{arrange.IsReadedAnswerByUser.Value}");
            Assert.True(arrange.IsReadedAnswerByUser.Value);
        }

        [InlineData(2)]
        [InlineData(3)]
        [Theory]
        public void DomainNotification_InvalidNotificationSenderId_Success(int notificationSenderId)
        {
            Assert.Throws<NotificationException>(() => _domainFactory.CreateDomainNotification(
               null,
               "a@gmail.com",
               null,
               null,
               "",
               notificationSenderId,
               1));
        }

        [InlineData(6)]
        [InlineData(5)]
        [Theory]
        public void DomainNotification_InvalidDomainNotificationStatusId_Success(int notificationStatusId)
        {
            Assert.Throws<NotificationException>(() => _domainFactory.CreateDomainNotification(
               null,
               "a@gmail.com",
               null,
               null,
               "",
               1,
               notificationStatusId));
        }

        [Fact]
        public void DomainUser_DomainNotification_Reference_Correct()
        {
            var user = _domainFactory.CreateDomainUser("a@gamil.com");
            var notification = _domainFactory.CreateDomainNotification(
               user.Id.Value,
               null,
               null,
               null,
               "",
               1,
               null);
            //Act
            user.AddNotifications([notification]);
            var result = user.Notifications.Values.Contains(notification);
            Assert.True(result);
        }

        [Fact]
        public void DomainUser_DomainNotification_Reference_InCorrectUserId()
        {
            var user = _domainFactory.CreateDomainUser("a@gamil.com");
            var notification = _domainFactory.CreateDomainNotification(
               Guid.NewGuid(),
               null,
               null,
               null,
               "",
               1,
               null);
            //Act
            user.AddNotifications([notification]);
            var result = user.Notifications.Values.Contains(notification);
            Assert.False(result);
        }

        [InlineData(5)]
        [InlineData(6)]
        [Theory]
        public void DomainUrl_InvalidUrlTypeId_UrlTypeException(int urlTypeId)
        {
            Assert.Throws<UrlTypeException>(() => _domainFactory.CreateDomainUrl(
                Guid.NewGuid(),
                urlTypeId,
                DateTime.Now,
                "https://www.youtube.com/",
                "Name",
                "Description"));

        }

        [InlineData("www.youtube.com/")]
        [InlineData("gmail.com")]
        [Theory]
        public void DomainUrl_InvalidUrlPath_UrlTypeException(string path)
        {
            Assert.Throws<UrlException>(() => _domainFactory.CreateDomainUrl(
                Guid.NewGuid(),
                1,
                DateTime.Now,
                path,
                "Name",
                "Description"));

        }

        [InlineData("https://pja.edu.pl/")]
        [InlineData("https://www.google.com/")]
        [Theory]
        public void DomainUrl_Update_Correct(string path)
        {
            var url = _domainFactory.CreateDomainUrl(
                Guid.NewGuid(),
                1,
                DateTime.Now,
                "https://www.youtube.com/",
                "Name",
                "Description");
            //Act
            url.Update(path, "new", "new");
            Assert.Equal(path, url.Path);
        }

        [Fact]
        public void DomainUrl_SeparateDuplicates_HasDuplicates()
        {
            var list = new List<DomainUrl>();
            var url = _domainFactory.CreateDomainUrl(
               Guid.NewGuid(),
               1,
               DateTime.Now,
               "https://www.youtube.com/",
               "Name",
               "Description");
            var url2 = _domainFactory.CreateDomainUrl(
               Guid.NewGuid(),
               1,
               DateTime.Now,
               "https://www.youtube.com/",
               "Name",
               "Description");
            list.Add(url);
            list.Add(url2);


            var act = DomainUrl.SeparateDuplicates(list).Duplicates.Count > 0;
            Assert.True(act);
        }

        [Fact]
        public void DomainUrl_SeparateDuplicates_HasNoDuplicates()
        {
            var list = new List<DomainUrl>();
            var url = _domainFactory.CreateDomainUrl(
               Guid.NewGuid(),
               1,
               DateTime.Now,
               "https://www.youtube2.com/",
               "Name",
               "Description");
            var url2 = _domainFactory.CreateDomainUrl(
               Guid.NewGuid(),
               1,
               DateTime.Now,
               "https://www.youtube.com/",
               "Name",
               "Description");
            list.Add(url);
            list.Add(url2);


            var act = DomainUrl.SeparateDuplicates(list).Duplicates.Count > 0;
            Assert.False(act);
        }

        [Fact]
        public void DomainUrl_FindConflictsWithDatabase_HasDuplicates()
        {
            var list = new List<DomainUrl>();
            var url = _domainFactory.CreateDomainUrl(
               Guid.NewGuid(),
               1,
               DateTime.Now,
               "https://www.youtube.com/",
               "Name",
               "Description");
            var url2 = _domainFactory.CreateDomainUrl(
               Guid.NewGuid(),
               1,
               DateTime.Now,
               "https://www.youtube2.com/",
               "Name",
               "Description");
            list.Add(url);
            list.Add(url2);

            var list2 = new List<DomainUrl>();
            var url3 = _domainFactory.CreateDomainUrl(
               Guid.NewGuid(),
               1,
               DateTime.Now,
               "https://www.youtube.com/",
               "Name",
               "Description");
            list2.Add(url3);


            var act = DomainUrl.FindConflictsWithDatabase(list, list2).Duplicates.Count > 0;
            Assert.True(act);
        }


        [Fact]
        public void DomainUrl_FindConflictsWithDatabase_HasNoDuplicates()
        {
            var list = new List<DomainUrl>();
            var url = _domainFactory.CreateDomainUrl(
               Guid.NewGuid(),
               1,
               DateTime.Now,
               "https://www.youtube.com/",
               "Name",
               "Description");
            var url2 = _domainFactory.CreateDomainUrl(
               Guid.NewGuid(),
               1,
               DateTime.Now,
               "https://www.youtube2.com/",
               "Name",
               "Description");
            list.Add(url);
            list.Add(url2);

            var list2 = new List<DomainUrl>();
            var url3 = _domainFactory.CreateDomainUrl(
               Guid.NewGuid(),
               1,
               DateTime.Now,
               "https://www.youtube3.com/",
               "Name",
               "Description");
            var url4 = _domainFactory.CreateDomainUrl(
               Guid.NewGuid(),
               1,
               DateTime.Now,
               "https://www.youtube4.com/",
               "Name",
               "Description");
            list2.Add(url3);
            list2.Add(url4);


            var act = DomainUrl.FindConflictsWithDatabase(list, list2).Duplicates.Count > 0;
            Assert.False(act);
        }

        [Fact]
        public void DomainUser_DomainUrl_References_Correct()
        {
            var user = _domainFactory.CreateDomainUser("a@gamil.com");
            var url = _domainFactory.CreateDomainUrl(
               user.Id.Value,
               1,
               DateTime.Now,
               "https://www.youtube.com/",
               "Name",
               "Description");

            user.AddUrls([url]);

            var act = user.Urls.Values.Contains(url);
            Assert.True(act);
        }

        [Fact]
        public void DomainUser_DomainUrl_References_InCorrect()
        {
            var user = _domainFactory.CreateDomainUser("a@gamil.com");
            var url = _domainFactory.CreateDomainUrl(
               Guid.NewGuid(),
               1,
               DateTime.Now,
               "https://www.youtube.com/",
               "Name",
               "Description");

            user.AddUrls([url]);

            var act = user.Urls.Values.Contains(url);
            Assert.False(act);
        }

        [Fact]
        public void DomainCompany_UpdateData_Correct()
        {
            var email = "z@gamil.com";
            var url = "a";
            var company = _domainFactory.CreateDomainCompany(
                Guid.NewGuid(),
                null,
                "a@gmail.com",
                "name",
                "123456789",
                "description");

            company.UpdateData(
                url,
                email,
                "newName",
                "newDescription");
            Assert.Equal(email, company.ContactEmail.Value);
            Assert.Equal(url, company.UrlSegment.Value);
        }

        [Fact]
        public void DomainUser_DomainCompany_References_Correct()
        {
            var user = _domainFactory.CreateDomainUser(
                "a@gamil.com");
            var company = _domainFactory.CreateDomainCompany(
                user.Id.Value,
                null,
                "a@gmail.com",
                "name",
                "123456789",
                "description");
            user.Company = company;

            Assert.Equal(user, company.User);
        }

        [Fact]
        public void DomainUser_DomainCompany_References_InCorrect()
        {
            var user = _domainFactory.CreateDomainUser(
                "a@gamil.com");
            var company = _domainFactory.CreateDomainCompany(
                Guid.NewGuid(),
                null,
                "a@gmail.com",
                "name",
                "123456789",
                "description");
            user.Company = company;
            var act = user == company.User;
            Assert.False(act);
        }

        [Fact]
        public void DomainBranch_Update_Correct()
        {
            var branch = _domainFactory.CreateDomainBranch(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "a",
                "name",
                "description");
            var newUrl = "new";
            var newName = "newName";
            var newDescription = "newDescription";
            branch.Update(
                Guid.NewGuid(),
                newUrl,
                newName,
                newDescription);

            Assert.Equal(newUrl, branch.UrlSegment.Value);
            Assert.Equal(newName, branch.Name);
            Assert.Equal(newDescription, branch.Description);
        }

        [Fact]
        public void DomainCompany_DomainBranch_References_Correct()
        {
            var company = _domainFactory.CreateDomainCompany(
                Guid.NewGuid(),
                null,
                "a@gamil.com",
                "name",
                "123456789",
                "description");
            var branch = _domainFactory.CreateDomainBranch(
                company.Id.Value,
                Guid.NewGuid(),
                "a",
                "name",
                "description");
            company.AddBranches([branch]);
            var act = branch.Company == company;
            Assert.True(act);
        }


        [Fact]
        public void DomainCompany_DomainBranch_References_InCorrect()
        {
            var company = _domainFactory.CreateDomainCompany(
                Guid.NewGuid(),
                null,
                "a@gamil.com",
                "name",
                "123456789",
                "description");
            var branch = _domainFactory.CreateDomainBranch(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "a",
                "name",
                "description");
            company.AddBranches([branch]);
            var act = branch.Company == company;
            Assert.False(act);
        }

        [Fact]
        public void DomainBranch_DomainAddress_References_Correct()
        {
            var address = _domainFactory.CreateDomainAddress(
                Guid.NewGuid(),
                1,
                1,
                "1",
                null,
                "12345",
                1,
                1);
            var branch = _domainFactory.CreateDomainBranch(
               Guid.NewGuid(),
               address.Id.Value,
               "a",
               "name",
               "description");
            branch.Address = address;
            var act = branch.Address == address;
            Assert.True(act);
        }

        [Fact]
        public void DomainBranch_DomainAddress_StreetNull_Correct()
        {
            var address = _domainFactory.CreateDomainAddress(
                Guid.NewGuid(),
                1,
                null,
                "1",
                null,
                "12345",
                1,
                1);
            var street = new DomainStreet(1, "", 1, "");
            address.Street = street;
            Assert.Null(address.StreetId);
            Assert.Null(address.Street);
            Assert.Null(street.StreetType);
        }

        [InlineData(null, "")]
        [InlineData(1, "")]
        [InlineData(1, "\n")]
        [InlineData(1, null)]
        [InlineData(null, null)]
        [Theory]
        public void DomainStreet_DomainAdministrativeType_IsNull(int? id, string? name)
        {
            var street = new DomainStreet(1, "", id, name);
            Assert.Null(street.StreetType);
        }

        [Fact]
        public void DomainBranch_DomainAddress_References_InCorrect()
        {
            var address = _domainFactory.CreateDomainAddress(
                Guid.NewGuid(),
                1,
                1,
                "1",
                null,
                "12345",
                1,
                1);
            var branch = _domainFactory.CreateDomainBranch(
               Guid.NewGuid(),
               Guid.NewGuid(),
               "a",
               "name",
               "description");
            Assert.Throws<AddressException>(() => branch.Address = address);
        }

        [Fact]
        public void DomainBranch_SeparateAndFilterBranches_HaveNoDuplicates()
        {
            var id = Guid.NewGuid();
            var branch = _domainFactory.CreateDomainBranch(
               id,
               Guid.NewGuid(),
               "a",
               "name",
               "description");
            var branch2 = _domainFactory.CreateDomainBranch(
               id,
               Guid.NewGuid(),
               "b",
               "name",
               "description");
            var branch3 = _domainFactory.CreateDomainBranch(
               id,
               Guid.NewGuid(),
               "c",
               "name",
               "description");
            var branch4 = _domainFactory.CreateDomainBranch(
               id,
               Guid.NewGuid(),
               null,
               "name",
               "description");
            var list = new List<DomainBranch>();
            list.Add(branch);
            list.Add(branch2);
            list.Add(branch3);
            list.Add(branch4);

            var act = DomainBranch.SeparateAndFilterBranches(list);
            Assert.False(act.HasDuplicates);
        }

        [Fact]
        public void DomainBranch_SeparateAndFilterBranches_HaveDuplicates()
        {
            var id = Guid.NewGuid();
            var branch = _domainFactory.CreateDomainBranch(
               id,
               Guid.NewGuid(),
               "a",
               "name",
               "description");
            var branch2 = _domainFactory.CreateDomainBranch(
               id,
               Guid.NewGuid(),
               "a",
               "name",
               "description");
            var branch3 = _domainFactory.CreateDomainBranch(
               id,
               Guid.NewGuid(),
               "c",
               "name",
               "description");
            var branch4 = _domainFactory.CreateDomainBranch(
               id,
               Guid.NewGuid(),
               null,
               "name",
               "description");
            var list = new List<DomainBranch>();
            list.Add(branch);
            list.Add(branch2);
            list.Add(branch3);
            list.Add(branch4);

            var act = DomainBranch.SeparateAndFilterBranches(list);
            Assert.True(act.HasDuplicates);
        }

        [Fact]
        public void DomainBranchOffer_Update_Correct()
        {
            var publishStart = DateTime.Now;
            var publishEnd = DateTime.Now;
            var workStart = DateOnly.FromDateTime(publishStart.AddDays(1));
            var workEnd = DateOnly.FromDateTime(publishStart.AddDays(2));
            var bo = _domainFactory.CreateDomainBranchOffer(
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.Now,
                null,
                null,
                null);
            bo.Update(publishStart, publishEnd, workStart, workEnd);
            Assert.Equal(workStart, bo.WorkStart);
            Assert.Equal(workEnd, bo.WorkEnd);
        }

        [Fact]
        public void DomainBranch_DomainBranchOffer_References_Correct()
        {
            var branch = _domainFactory.CreateDomainBranch(
               Guid.NewGuid(),
               Guid.NewGuid(),
               "a",
               "name",
               "description");
            var bo = _domainFactory.CreateDomainBranchOffer(
                branch.Id.Value,
                Guid.NewGuid(),
                DateTime.Now,
                null,
                null,
                null);
            branch.AddBranchOffers([bo]);
            var act = bo.Branch == branch;
            Assert.True(act);
        }

        [Fact]
        public void DomainBranch_DomainBranchOffer_References_InCorrect()
        {
            var branch = _domainFactory.CreateDomainBranch(
               Guid.NewGuid(),
               Guid.NewGuid(),
               "a",
               "name",
               "description");
            var bo = _domainFactory.CreateDomainBranchOffer(
               Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.Now,
                null,
                null,
                null);
            branch.AddBranchOffers([bo]);
            var act = bo.Branch == branch;
            Assert.False(act);
        }

        [Fact]
        public void DomainBranchOffer_ReturnDuplicatesAndCorrectValues_InCorrect()
        {
            var branchId = Guid.NewGuid();
            var offerId = Guid.NewGuid();
            var bo = _domainFactory.CreateDomainBranchOffer(
                branchId,
                offerId,
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(1),
                null,
                null);
            var bo2 = _domainFactory.CreateDomainBranchOffer(
                branchId,
                offerId,
                DateTime.Now.AddHours(1),
                DateTime.Now.AddDays(1),
                null,
                null);
            var bo3 = _domainFactory.CreateDomainBranchOffer(
                branchId,
                offerId,
                DateTime.Now,
                DateTime.Now,
                null,
                null);
            var list = new List<DomainBranchOffer>();
            list.Add(bo);
            list.Add(bo2);
            list.Add(bo3);

            var result = DomainBranchOffer.SeparateAndFilterBranchOffers(list);
            var act = result.Conflicts.Count() > 0;
            _output.WriteLine($"Total: {list.Count}, Correct; {result.Correct.Count()}, InCorrrect: {result.Conflicts.Count()} ");
            Assert.True(act);
        }

        [Fact]
        public void DomainBranchOffer_ReturnDuplicatesAndCorrectValues_WithoutDuration()
        {
            var now = DateTime.Now;
            var branchId = Guid.NewGuid();
            var offerId = Guid.NewGuid();
            var bo = _domainFactory.CreateDomainBranchOffer(
                branchId,
                offerId,
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(1),
                null,
                null);
            var bo2 = _domainFactory.CreateDomainBranchOffer(
                branchId,
                offerId,
                now,
                now,
                null,
                null);
            var list = new List<DomainBranchOffer>();
            list.Add(bo);
            list.Add(bo2);

            var result = DomainBranchOffer.SeparateAndFilterBranchOffers(list);
            var act = result.WithoutDuration.Count() > 0;
            _output.WriteLine($"Total: {list.Count}, Correct;WDL: {result.WithoutDuration.Count()}  {result.Correct.Count()}, InCorrrect: {result.Conflicts.Count()} ");
            Assert.True(act);
        }

        [Fact]
        public void DomainAddress_DomainAdministrativeDivision_Correct()
        {
            var street = new DomainStreet(1, "S Name", null, null);
            var div = new DomainAdministrativeDivision(
                1,
                "D Name",
                null,
                1,
                "Div Name");
            var div2 = new DomainAdministrativeDivision(
                2,
                "D Name",
                1,
                2,
                "Div Name");
            var div3 = new DomainAdministrativeDivision(
                3,
                "D Name",
                2,
                3,
                "Div Name");
            var dik = new Dictionary<DivisionId, DomainAdministrativeDivision>()
            {
                { div.Id, div},
                { div2.Id, div2},
                { div3.Id, div3},
            };
            var address = _domainFactory.CreateDomainAddress(
                null,
                div3.Id.Value,
                street.Id.Value,
                "1",
                null,
                "12345",
                1,
                1);

            address.Street = street;
            address.SetHierarchy(dik);

            var act = address.Hierarchy.Count() == dik.Count();
            Assert.True(act);
        }

        [Fact]
        public void DomainAddress_DomainStreet_Correct()
        {
            var street = new DomainStreet(1, "S Name", null, null);
            var div = new DomainAdministrativeDivision(
                1,
                "D Name",
                null,
                1,
                "Div Name");
            var div2 = new DomainAdministrativeDivision(
                2,
                "D Name",
                1,
                2,
                "Div Name");
            var div3 = new DomainAdministrativeDivision(
                3,
                "D Name",
                2,
                3,
                "Div Name");
            var dik = new Dictionary<DivisionId, DomainAdministrativeDivision>()
            {
                { div.Id, div},
                { div2.Id, div2},
                { div3.Id, div3},
            };
            var address = _domainFactory.CreateDomainAddress(
                null,
                div3.Id.Value,
                street.Id.Value,
                "1",
                null,
                "12345",
                1,
                1);

            address.Street = street;
            address.SetHierarchy(dik);

            var act = address.Street == street;
            Assert.True(act);
        }

        [Fact]
        public void DomainAddress_DomainStreet_InCorrect()
        {
            var street = new DomainStreet(1, "S Name", null, null);

            var address = _domainFactory.CreateDomainAddress(
                null,
                1,
                2,
                "1",
                null,
                "12345",
                1,
                1);

            Assert.Throws<AddressException>(() => address.Street = street);
        }

        [Fact]
        public void DomainAddress_DomainAdministrativeDivision_InCorrect()
        {
            var div = new DomainAdministrativeDivision(
                1,
                "D Name",
                null,
                1,
                "Div Name");
            var div2 = new DomainAdministrativeDivision(
                2,
                "D Name",
                1,
                2,
                "Div Name");
            var div3 = new DomainAdministrativeDivision(
                3,
                "D Name",
                2,
                3,
                "Div Name");
            var dik = new Dictionary<DivisionId, DomainAdministrativeDivision>()
            {
                { div.Id, div},
                { div2.Id, div2},
                { div3.Id, div3},
            };
            var address = _domainFactory.CreateDomainAddress(
                null,
                4,
                4,
                "1",
                null,
                "12345",
                1,
                1);

            Assert.Throws<AddressException>(() => address.SetHierarchy(dik));
        }


        [Fact]
        public void DomainOffer_Update_Correct()
        {
            var offer = _domainFactory.CreateDomainOffer(
                "name",
                "description",
                1,
                2,
                "Y",
                "Y");
            offer.Update(
                "new",
                "new",
                2,
                3,
                false,
                false);
            Assert.Equal(false, offer.IsForStudents.Value);
            Assert.Equal(false, offer.IsNegotiatedSalary.Value);
            Assert.Equal(true, offer.IsPaid);
        }

        [Fact]
        public void DomainOffer_Update_MoneyException()
        {
            var offer = _domainFactory.CreateDomainOffer(
                "name",
                "description",
                1,
                2,
                "Y",
                "Y");
            ;
            Assert.Throws<MoneyException>(() => offer.Update(
                "new",
                "new",
                4,
                3,
                false,
                false));
        }

        [Fact]
        public void DomainOffer_Update_OfferException()
        {
            var offer = _domainFactory.CreateDomainOffer(
                "name",
                "description",
                1,
                2,
                "Y",
                "Y");
            ;
            Assert.Throws<OfferException>(() => offer.Update(
                "new",
                "new",
                2,
                3,
                null,
                false));
        }

        [Fact]
        public void DomainBranchOffer_DomainOffer_Reference_Correct()
        {
            var branchId = Guid.NewGuid();
            var offer = _domainFactory.CreateDomainOffer(
                "name",
                "description",
                1,
                2,
                "Y",
                "Y");
            ;
            var bo = _domainFactory.CreateDomainBranchOffer(
                branchId,
                offer.Id.Value,
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(1),
                null,
                null);

            offer.SetBranchOffers([bo]);
            var act = bo.Offer == offer || offer.BranchOffers.Values.Contains(bo);
            Assert.True(act);
        }

        [Fact]
        public void DomainBranchOffer_DomainOffer_Reference_InCorrect()
        {
            var branchId = Guid.NewGuid();
            var offerId = Guid.NewGuid();
            var offer = _domainFactory.CreateDomainOffer(
                "name",
                "description",
                1,
                2,
                "Y",
                "Y");
            ;
            var bo = _domainFactory.CreateDomainBranchOffer(
                branchId,
                offerId,
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(1),
                null,
                null);

            offer.SetBranchOffers([bo]);
            var act = bo.Offer == offer;
            Assert.False(act);
        }

        [Fact]
        public void DomainOffer_SetCharacteristics_Correct()
        {
            var offer = _domainFactory.CreateDomainOffer(
                "name",
                "description",
                1,
                2,
                "Y",
                "Y");
            ;
            var list = new List<(CharacteristicId, QualityId?)>();
            list.Add((new CharacteristicId(1), new QualityId(1)));
            list.Add((new CharacteristicId(2), new QualityId(3)));
            list.Add((new CharacteristicId(3), new QualityId(3)));

            offer.SetCharacteristics(list);
            var act = offer.Characteristics.Count() == 2;
            Assert.True(act);
        }

        [Fact]
        public void DomainPerson_Update_Correct()
        {
            var person = _domainFactory.CreateDomainPerson(
                Guid.NewGuid(),
                "a",
                "a@gmail.com",
                "Name",
                "Surname",
                new DateOnly(2000, 1, 1),
                "123456789",
                "Description",
                true,
                true,
                Guid.NewGuid());

            var newEmail = "b@gamail.com";
            person.Update(
                null,
                newEmail,
                "A",
                "Sur",
                null,
                null,
                null,
                true,
                true,
                null);
            Assert.Equal(newEmail, person.ContactEmail);
            Assert.Equal(null, person.UrlSegment);
        }

        [Fact]
        public void DomainPerson_DomainUser_References_Correct()
        {
            var user = _domainFactory.CreateDomainUser("a@gamil.com");
            var person = _domainFactory.CreateDomainPerson(
                user.Id.Value,
                "a",
                "a@gmail.com",
                "Name",
                "Surname",
                new DateOnly(2000, 1, 1),
                "123456789",
                "Description",
                true,
                true,
                Guid.NewGuid());
            person.User = user;
            var act = user.Person == person;

            Assert.True(act);
        }

        [Fact]
        public void DomainPerson_DomainUser_References_InCorrect()
        {
            var user = _domainFactory.CreateDomainUser("a@gamil.com");
            var person = _domainFactory.CreateDomainPerson(
                Guid.NewGuid(),
                "a",
                "a@gmail.com",
                "Name",
                "Surname",
                new DateOnly(2000, 1, 1),
                "123456789",
                "Description",
                true,
                true,
                Guid.NewGuid());
            person.User = user;
            var act = user.Person == null;
            Assert.True(act);
        }

        [Fact]
        public void DomainPerson_DomainAddress_References_Correct()
        {
            var address = _domainFactory.CreateDomainAddress(
                null,
                1,
                1,
                "1",
                null,
                "12345",
                1,
                1);
            var person = _domainFactory.CreateDomainPerson(
                Guid.NewGuid(),
                "a",
                "a@gmail.com",
                "Name",
                "Surname",
                new DateOnly(2000, 1, 1),
                "123456789",
                "Description",
                true,
                true,
                address.Id.Value);
            person.Address = address;
            var act = person.Address == address;
            Assert.True(act);
        }

        [Fact]
        public void DomainPerson_DomainAddress_References_InCorrect()
        {
            var address = _domainFactory.CreateDomainAddress(
                null,
                1,
                1,
                "1",
                null,
                "12345",
                1,
                1);
            var person = _domainFactory.CreateDomainPerson(
                Guid.NewGuid(),
                "a",
                "a@gmail.com",
                "Name",
                "Surname",
                new DateOnly(2000, 1, 1),
                "123456789",
                "Description",
                true,
                true,
                Guid.NewGuid());
            Assert.Throws<AddressException>(() => person.Address = address);
        }

        [Fact]
        public void DomainPerson_SetCharacteristics_Correct()
        {
            var person = _domainFactory.CreateDomainPerson(
                Guid.NewGuid(),
                "a",
                "a@gmail.com",
                "Name",
                "Surname",
                new DateOnly(2000, 1, 1),
                "123456789",
                "Description",
                true,
                true,
                Guid.NewGuid());
            var list = new List<(CharacteristicId, QualityId?)>();
            list.Add((new CharacteristicId(1), new QualityId(1)));
            list.Add((new CharacteristicId(2), new QualityId(3)));
            list.Add((new CharacteristicId(3), new QualityId(3)));

            person.SetCharacteristics(list);
            var act = person.Characteristics.Count() == 2;
            Assert.True(act);
        }

        [Fact]
        public void DomainRecruitment_SetAnswer_Correct()
        {
            var recruitment = _domainFactory.CreateDomainRecruitment(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Message");
            recruitment.SetAnswer("Response", true);
            Assert.True(recruitment.IsAccepted.Value);
            Assert.Equal("Response", recruitment.CompanyResponse);
        }

        [Fact]
        public void DomainPerson_DomainRecruitment_Reference_Correct()
        {
            var person = _domainFactory.CreateDomainPerson(
                Guid.NewGuid(),
                "a",
                "a@gmail.com",
                "Name",
                "Surname",
                new DateOnly(2000, 1, 1),
                "123456789",
                "Description",
                true,
                true,
                Guid.NewGuid());
            var recruitment = _domainFactory.CreateDomainRecruitment(
                person.Id.Value,
                Guid.NewGuid(),
                "Message");
            person.SetRecrutment(recruitment);
            var act = recruitment.Person == person;
            Assert.True(act);
        }

        [Fact]
        public void DomainPerson_DomainRecruitment_Reference_InCorrect()
        {
            var person = _domainFactory.CreateDomainPerson(
                Guid.NewGuid(),
                "a",
                "a@gmail.com",
                "Name",
                "Surname",
                new DateOnly(2000, 1, 1),
                "123456789",
                "Description",
                true,
                true,
                Guid.NewGuid());
            var recruitment = _domainFactory.CreateDomainRecruitment(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Message");
            person.SetRecrutment(recruitment);
            var act = recruitment.Person == null;
            Assert.True(act);
        }

        [Fact]
        public void DomainBranchOffer_DomainRecruitment_Reference_Correct()
        {
            var bo = _domainFactory.CreateDomainBranchOffer(
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.Now,
                null,
                null,
                null);
            var recruitment = _domainFactory.CreateDomainRecruitment(
                Guid.NewGuid(),
                bo.Id.Value,
                "Message");
            recruitment.BranchOffer = bo;
            var act = bo.Recrutments.ContainsKey(recruitment.Id);
            Assert.True(act);
        }

        [Fact]
        public void DomainBranchOffer_DomainRecruitment_Reference_InCorrect()
        {
            var bo = _domainFactory.CreateDomainBranchOffer(
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.Now,
                null,
                null,
                null);
            var recruitment = _domainFactory.CreateDomainRecruitment(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Message");
            recruitment.BranchOffer = bo;
            var act = !bo.Recrutments.ContainsKey(recruitment.Id);
            Assert.True(act);
        }

        [Fact]
        public void DomainIntership_Update_Correct()
        {
            var internship = _domainFactory.CreateDomainInternship(
                Guid.NewGuid(),
                DateOnly.FromDateTime(DateTime.Now),
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                "a");
            internship.Update(
                "b",
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                null);
            Assert.Equal("b", internship.ContractNumber.Value);
            Assert.Equal(null, internship.ContractEndDate);
        }

        [Fact]
        public void DomainIntership_DomainRecruitment_Reference_Correct()
        {
            var recruitment = _domainFactory.CreateDomainRecruitment(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Message");
            var internship = _domainFactory.CreateDomainInternship(
                recruitment.Id.Value,
                DateOnly.FromDateTime(DateTime.Now),
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                "a");
            internship.Recrutment = recruitment;
            var act = recruitment.Intership == internship;
            Assert.True(act);
        }

        [Fact]
        public void DomainIntership_DomainRecruitment_Reference_InCorrect()
        {
            var recruitment = _domainFactory.CreateDomainRecruitment(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Message");
            var internship = _domainFactory.CreateDomainInternship(
                Guid.NewGuid(),
                DateOnly.FromDateTime(DateTime.Now),
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                "a");
            internship.Recrutment = recruitment;
            var act = recruitment.Intership == null;
            Assert.True(act);
        }

        [Fact]
        public void DomainIntership_DomainComment_References_Correct()
        {
            var internship = _domainFactory.CreateDomainInternship(
                Guid.NewGuid(),
                DateOnly.FromDateTime(DateTime.Now),
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                "a");
            var comment = _domainFactory.CreateDomainComment(
                internship.Id.Value,
                Domain.Features.Comment.ValueObjects.CommentTypePart.CommentSenderEnum.Person,
                Domain.Features.Comment.ValueObjects.CommentTypePart.CommentTypeEnum.Dzienny,
                "Description",
                5);
            internship.AddComment(comment);
            var act = comment.Intership == internship;
            Assert.True(act);
        }

        [Fact]
        public void DomainIntership_DomainComment_References_InCorrect()
        {
            var internship = _domainFactory.CreateDomainInternship(
                Guid.NewGuid(),
                DateOnly.FromDateTime(DateTime.Now),
                DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                "a");
            var comment = _domainFactory.CreateDomainComment(
                Guid.NewGuid(),
                1,
                DateTime.Now,
                "Description",
                5);
            internship.AddComments([comment]);
            var act = !internship.Comments.ContainsKey(comment.Id);
            Assert.True(act);
        }

        [Fact]
        public void DomainComment_NullEvaluation_CommentEvaluationException()
        {
            Assert.Throws<CommentEvaluationException>(() => _domainFactory.CreateDomainComment(
                Guid.NewGuid(),
                Domain.Features.Comment.ValueObjects.CommentTypePart.CommentSenderEnum.Person,
                Domain.Features.Comment.ValueObjects.CommentTypePart.CommentTypeEnum.Dzienny,
                "Description",
                null));
        }

        [Fact]
        public void DomainCharacteristicType_SetCharacteristics_Corrrect()
        {
            var provider = new Provider(
                    new ExceptionMessageProvider(),
                    new Domain.Shared.Providers.Time.TimeProvider());
            var type = new DomainCharacteristicType(
                1,
                "Name",
                "Desc",
                provider);

            var characteristic1 = new DomainCharacteristic(
                1,
                "Name1",
                "Desc",
                1,
                [],
                provider);
            var characteristic2 = new DomainCharacteristic(
                2,
                "Name2",
                "Desc",
                1,
                [],
                provider);
            var characteristic3 = new DomainCharacteristic(
                3,
                "Name2",
                "Desc",
                2,
                [],
                provider);
            var list = new List<DomainCharacteristic>();
            list.Add(characteristic1);
            list.Add(characteristic2);
            list.Add(characteristic3);
            type.SetCharacteristics(list);
            Assert.Equal(2, type.CharacteristicDictionary.Count());
        }

        [Fact]
        public void DomainCharacteristicType_SetQualities_Correct()
        {
            var provider = new Provider(
                    new ExceptionMessageProvider(),
                    new Domain.Shared.Providers.Time.TimeProvider());
            var type = new DomainCharacteristicType(
                1,
                "Name",
                "Desc",
                provider);

            var quality1 = new DomainQuality(
                1,
                "Name1",
                "Desc",
                provider);
            var quality2 = new DomainQuality(
                2,
                "Name2",
                "Desc",
                provider);
            var quality3 = new DomainQuality(
                3,
                "Name2",
                "Desc",
                provider);
            var list = new List<DomainQuality>();
            list.Add(quality1);
            list.Add(quality2);
            list.Add(quality3);
            type.SetQualities(list);
            Assert.Equal(3, type.QualityDictionary.Count());
        }

        [Fact]
        public void DomainCharacteristic_GetQuality_Correct()
        {
            var provider = new Provider(
                    new ExceptionMessageProvider(),
                    new Domain.Shared.Providers.Time.TimeProvider());
            var type = new DomainCharacteristicType(
                1,
                "Name",
                "Desc",
                provider);

            var quality1 = new DomainQuality(
                1,
                "Name1",
                "Desc",
                provider);
            var quality2 = new DomainQuality(
                2,
                "Name2",
                "Desc",
                provider);
            var quality3 = new DomainQuality(
                3,
                "Name2",
                "Desc",
                provider);
            var listQualities = new List<DomainQuality>();
            listQualities.Add(quality1);
            listQualities.Add(quality2);
            listQualities.Add(quality3);
            type.SetQualities(listQualities);
            Assert.Equal(3, type.QualityDictionary.Count());

            var characteristic1 = new DomainCharacteristic(
                 1,
                 "Name1",
                 "Desc",
                 1,
                 [],
                 provider);
            var characteristic2 = new DomainCharacteristic(
                2,
                "Name2",
                "Desc",
                1,
                [],
                provider);
            var characteristic3 = new DomainCharacteristic(
                3,
                "Name2",
                "Desc",
                2,
                [],
                provider);
            var listCharacteristics = new List<DomainCharacteristic>();
            listCharacteristics.Add(characteristic1);
            listCharacteristics.Add(characteristic2);
            listCharacteristics.Add(characteristic3);
            type.SetCharacteristics(listCharacteristics);

            var act = characteristic1.GetQuality(new QualityId(1));

            Assert.Equal(quality1, act);
        }

        [Fact]
        public void DomainCharacteristic_GetQuality_InCorrect()
        {
            var provider = new Provider(
                    new ExceptionMessageProvider(),
                    new Domain.Shared.Providers.Time.TimeProvider());
            var type = new DomainCharacteristicType(
                1,
                "Name",
                "Desc",
                provider);

            var quality1 = new DomainQuality(
                1,
                "Name1",
                "Desc",
                provider);
            var quality2 = new DomainQuality(
                2,
                "Name2",
                "Desc",
                provider);
            var quality3 = new DomainQuality(
                3,
                "Name2",
                "Desc",
                provider);
            var listQualities = new List<DomainQuality>();
            listQualities.Add(quality1);
            listQualities.Add(quality2);
            listQualities.Add(quality3);
            type.SetQualities(listQualities);
            Assert.Equal(3, type.QualityDictionary.Count());

            var characteristic1 = new DomainCharacteristic(
                 1,
                 "Name1",
                 "Desc",
                 1,
                 [],
                 provider);
            var characteristic2 = new DomainCharacteristic(
                2,
                "Name2",
                "Desc",
                1,
                [],
                provider);
            var characteristic3 = new DomainCharacteristic(
                3,
                "Name2",
                "Desc",
                2,
                [],
                provider);
            var listCharacteristics = new List<DomainCharacteristic>();
            listCharacteristics.Add(characteristic1);
            listCharacteristics.Add(characteristic2);
            listCharacteristics.Add(characteristic3);
            type.SetCharacteristics(listCharacteristics);

            Assert.Throws<QualityException>(() => characteristic1.GetQuality(new QualityId(4)));
        }
        //DomainCharacteristicks
        //DomainCharacteristicksType

    }
}