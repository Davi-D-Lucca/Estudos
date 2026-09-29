using System;
using System.Collections.Generic;
using System.Text;

namespace EmprestimoLivroApi.Domain.Storage
{
    public interface IStorageService
    {
        Task<string> UploadPhotoBook(Stream file, string fileName);
        Stream? GetBookPhoto(string photoPath);
    }
}
