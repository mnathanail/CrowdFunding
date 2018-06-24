using Microsoft.AspNetCore.Http;
using Crowdfunding.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Crowdfunding.Utils
{
    public static class MediaChecking
    {
        public static Boolean checkPhotosContenType(IFormFileCollection photos)
        {
            var count = 0;
            var listLength = photos.Count();
            foreach(var photo in photos)
            {
                var checkContentType = photo.ContentType.ToLower() != "image/jpg" &&
                                       photo.ContentType.ToLower() != "image/jpeg" &&
                                       photo.ContentType.ToLower() != "image/pjpeg" &&
                                       photo.ContentType.ToLower() != "image/gif" &&
                                       photo.ContentType.ToLower() != "image/x-png" &&
                                       photo.ContentType.ToLower() != "image/png";
                if (!checkContentType) {
                    count++;
                }
            }
            return count == listLength ? true:false;
        }

        public static Boolean checkPhotosExtension(IFormFileCollection photos)
        {
            var count = 0;
            var listLength = photos.Count();
            foreach (var photo in photos)
            {
                var checkExtension = Path.GetExtension(photo.FileName).ToLower() != ".jpg" &&
                                     Path.GetExtension(photo.FileName).ToLower() != ".png" &&
                                     Path.GetExtension(photo.FileName).ToLower() != ".gif" &&
                                     Path.GetExtension(photo.FileName).ToLower() != ".jpeg";
                if (!checkExtension)
                {
                    count++;
                }
            }
            return count == listLength ? true : false;
        }

        public static Boolean checkVideoContenType(IFormFileCollection video)
        {
            var checkContentType = video.First().ContentType.ToLower() != "video/mp4" &&
                                   video.First().ContentType.ToLower() != "video/webm" &&
                                   video.First().ContentType.ToLower() != "video/ogg" &&
                                   video.First().ContentType.ToLower() != "application/ogg";
            return checkContentType;
        }

        public static Boolean checkVideoExtension(IFormFileCollection video)
        {
            var checkExtension = Path.GetExtension(video.First().FileName).ToLower() != ".flv" &&
                                 Path.GetExtension(video.First().FileName).ToLower() != ".mp4" &&
                                 Path.GetExtension(video.First().FileName).ToLower() != ".mov" &&
                                 Path.GetExtension(video.First().FileName).ToLower() != ".wmv" &&
                                 Path.GetExtension(video.First().FileName).ToLower() != ".avi";
            return checkExtension;
        }
    }
}
