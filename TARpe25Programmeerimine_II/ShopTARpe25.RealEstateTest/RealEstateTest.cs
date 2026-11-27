using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;

namespace ShopTARpe25.RealEstateTest
{
    public class RealEstateTest : TestBase
    {
        [Fact]
        public async Task ShouldNot_AddEmptyRealEstate_WhenReturnResult()
        {
            // Arrange
            RealEstateDto dto = new()
            {
                Area = 120.5,
                Address = "Test location",
                RoomNumber = 3,
                BuildingType = "house",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
            };

            var result = await Svc<IRealEstateServices>().Create(dto);

            Assert.NotNull(result);
        }
        [Fact]
        public async Task ShouldNot_GetByIdRealEstate_WhenReturnsNotEqual()
        {
            // Arrange
            Guid dataBaseGuid = Guid.Parse("5248caec-c78f-4c93-aa95-26be90ff57f0");
            Guid wrongGuid = Guid.NewGuid();

            // Act
            await Svc<IRealEstateServices>().DetailAsync(dataBaseGuid);

            // Assert
            Assert.NotEqual(dataBaseGuid, wrongGuid);
        }
        [Fact]
        public async Task Should_GetByIdRealestate_WhenReturnsEqual()
        {
            // Arrange

            Guid databaseGuid = Guid.Parse("5248caec-c78f-4c93-aa95-26be90ff57f0");
            Guid guid = Guid.Parse("5248caec-c78f-4c93-aa95-26be90ff57f0");

            // Act
            await Svc<IRealEstateServices>().DetailAsync(guid);

            // Assert
            Assert.Equal(databaseGuid, guid);
        }
        [Fact]
        public async Task Should_DeleteByIdRealEstate_WhenDeleteRealEstate()
        {
            // arrange
            RealEstateDto dto = MockRealEstateData();

            // act
            var realEstate1 = await Svc<IRealEstateServices>().Create(dto);
            var realEstate2 = await Svc<IRealEstateServices>().Create(dto);

            var result = await Svc<IRealEstateServices>().Delete((Guid)realEstate2.Id);

            // assert
            Assert.NotEqual(realEstate1.Id, result.Id);
        }
        [Fact]
        public async Task Should_UpdateRealEstate_WhenUpdatedData()
        {
            //arrange
            var guid = new Guid("5248caec-c78f-4c93-aa95-26be90ff57f0");

            RealEstateDto dto = MockRealEstateData();

            RealEstateDto domain = new();

            domain.Id = guid;
            domain.Area = 200.0;
            domain.Address = "Update Location";
            domain.RoomNumber = 5;
            domain.BuildingType = "Villa";
            domain.CreatedAt = DateTime.UtcNow;
            domain.ModifiedAt = DateTime.UtcNow;

            //act
            await Svc<IRealEstateServices>().Update(dto);

            //assert
            Assert.Equal(domain.Id, guid);
            Assert.NotEqual(dto.Area, domain.Area);
            Assert.NotEqual(dto.RoomNumber, domain.RoomNumber);
            Assert.DoesNotMatch(dto.RoomNumber.ToString(), domain.RoomNumber.ToString());
            Assert.DoesNotMatch(dto.Address.ToString(), domain.Address);
        }
        [Fact]
        public async Task Should_UpdateRealEstate_WhenUpdatedDataVersion2()
        {
            // Arrange
            RealEstateDto dto = MockRealEstateData2();
            RealEstateDto updateDto = MockRealEstateDataUpdate();

            var createdRealEstate = await Svc<IRealEstateServices>().Create(dto);
            var result = await Svc<IRealEstateServices>().Update(updateDto);

            // Assert
            Assert.DoesNotMatch(createdRealEstate.Address, result.Address);
            Assert.NotEqual(dto.ModifiedAt, result.ModifiedAt);
        }

        //kasutame ankeeti, kus kõik andmed on NULL
        //tuleb kasutada Create ja Update ära

        [Fact]
        public async Task Should_UpdateRealEstate_WhenDataIsNull()
        {
            // Arrange
            RealEstateDto dto = MockNullRealEstateData();
            RealEstateDto nullDto = MockRealEstateDataUpdate();

            // Act
            var createdRealEstate = await Svc<IRealEstateServices>().Create(dto);
            var result = await Svc<IRealEstateServices>().Update(nullDto);


            // Assert
            Assert.NotEqual(createdRealEstate.Id , result.Id);
            Assert.NotEqual(createdRealEstate.Address, result.Address);
        }

        private RealEstateDto MockNullRealEstateData()
        {
            return new RealEstateDto
            {
                Area = null,
                Address = null,
                RoomNumber = 0,
                BuildingType = null,
                CreatedAt = null,
                ModifiedAt = null
            };
        }

        private RealEstateDto MockRealEstateData()
        {
            return new RealEstateDto
            {
                Area = 88.65,
                Address = "TestAddress",
                RoomNumber = 1,
                BuildingType = "TestBuildingType",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };
        }

        private RealEstateDto MockRealEstateData2()
        {
            return new RealEstateDto
            {
                Area = 120.00,
                Address = "Sample Address",
                RoomNumber = 8,
                BuildingType = "House",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };
        }

        private RealEstateDto MockRealEstateDataUpdate()
        {
            return new RealEstateDto
            {
                Area = 100.00,
                Address = "Woodland",
                RoomNumber = 2,
                BuildingType = "Hideout",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };
        }
    }
}
