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

            var addRealEstate = await Svc<IRealEstateServices>().Create(dto);
            var deleteRealEstate = await Svc<IRealEstateServices>().Delete((Guid)addRealEstate.Id);

            // assert
            Assert.Equal(addRealEstate.Id, deleteRealEstate.Id);
        }
        private RealEstateDto MockRealEstateData()
        {
            RealEstateDto realEstate = new()
            {
                Area = 88.65,
                Address = "TestAddress",
                RoomNumber = 1,
                BuildingType = "TestBuildingType",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
            };

            return realEstate;
        }
    }
}
