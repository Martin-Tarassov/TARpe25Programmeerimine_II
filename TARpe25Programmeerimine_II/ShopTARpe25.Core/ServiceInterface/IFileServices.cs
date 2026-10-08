using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopTARpe25.Core.ServiceInterface
{
    public interface IFileServices
    {
        void FilesToApi(SpaceshipDto dto, Spaceship domain);
        Task<FileToApi> RemoveImageFromApi(FileToApiDto dto);
    }
}
