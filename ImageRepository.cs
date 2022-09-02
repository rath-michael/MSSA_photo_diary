using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Week8Project
{
    internal class Dict
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }
    internal static class ImageRepository
    {
        private static Week8ProjectEntities entities = new Week8ProjectEntities();
        // Get all images associated with specified state
        public static List<UserImage> GetImagesByState(string stateName)
        {
            List<UserImage> images = null;
            try
            {
                images = (from state in entities.States
                           join image in entities.UserImages
                           on state equals image.State
                           where state.StateName == stateName
                           select image).ToList();

                Console.WriteLine("ImageRepository GetImagesByStates() success");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ImageRepository GetImagesByStates(): " + ex.Message);
            }
            return images;
        }

        // Get count of photos per state from database
        //public static async Task<IEnumerable<Dict>> GetCount()
        //{
        //    IEnumerable<Dict> res = null;
        //    try
        //    {
        //        res = (from state in entities.States
        //               join image in entities.UserImages
        //               on state.StateID equals image.StateID
        //               group state by state.StateName into grp
        //               select new Dict { Name = grp.Key, Count = grp.Count() }).AsParallel();
        //        Console.WriteLine("ImageRepository GetCount() success");
        //        return res.ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("ImageRepository GetCount(): " + ex.Message);
        //    }
        //    return null;
        //}
        public static async Task<IEnumerable<Dict>> GetPhotoCount()
        {
            IEnumerable<Dict> res = null;
            try
            {
                res = await (from state in entities.States
                       join image in entities.UserImages
                       on state.StateID equals image.StateID
                       group state by state.StateName into grp
                       select new Dict { Name = grp.Key, Count = grp.Count() }).ToListAsync();
                Console.WriteLine("ImageRepository GetPhotoCount() success");
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ImageRepository GetPhotoCount(): " + ex.Message);
            }
            return null;
        }
        // Search for list of images by given search term
        public static List<UserImage> GetImagesByTerm(string searchTerm)
        {
            List<UserImage> images = null;
            try
            {
                images = (from image in entities.UserImages
                          join tag in entities.ImageTags
                          on image.ImageID equals tag.UserImageID
                          join state in entities.States
                          on image.StateID equals state.StateID
                          where tag.Tag.ToLower().Contains(searchTerm.ToLower())
                          || state.StateName.ToLower().Contains(searchTerm.ToLower())
                          select image).Distinct().ToList();
                Console.WriteLine("ImageRepository GetImagesByTerm() success");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ImageRepository GetImagesByTerm(): " + ex.Message);
            }
            return images;
        }
        // Get list of all states
        public static List<State> GetStates()
        {
            List<State> states = null;
            try
            {
                states = (from state in entities.States
                          select state).ToList();
                Console.WriteLine("ImageRepository GetStates() success");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ImageRepository GetStates(): " + ex.Message);
            }
            return states;
        }
        // Get ID of specified state
        public static int GetStateID(string stateName)
        {
            int id = -1;
            try
            {
                id = (from s in entities.States
                      where s.StateName == stateName
                      select s.StateID).Single();
                Console.WriteLine("ImageRepository GetStateID() success");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ImageRepository GetStateID(): " + ex.Message);
            }
            return id;
        }
        // Get ID of most recently added image
        public static int GetMostRecentImageID()
        {
            return entities.UserImages.Max(x => x.ImageID);
        }
        // Get ID of most recently added tag
        public static int GetMostRecentTagID()
        {
            return entities.ImageTags.Max(x => x.TagID);
        }
        // Add specified image
        public static bool AddImage(UserImage image)
        {
            bool res = false;
            try
            {
                entities.UserImages.Add(image);
                entities.SaveChanges();
                res = true;
                Console.WriteLine("ImageRepository AddImage() success");
            }
            catch (Exception ex)
            {
                res = false;
                Console.WriteLine("ImageRepository AddImage(): " + ex.Message);
            }
            return res;
        }
        // Remove specified image
        public static bool DeleteImage(UserImage image)
        {
            bool res = false;
            try
            {
                entities.UserImages.Remove(image);
                entities.SaveChanges();
                res = true;
                Console.WriteLine("ImageRepository DeleteImage() success");
            }
            catch (Exception ex)
            {
                res = false;
                Console.WriteLine("ImageRepository DeleteImage(): " + ex.Message);
            }
            return res;
        }
        // Update specified image
        public static bool UpdateImage(UserImage updatedImage)
        {
            bool res = false;
            try
            {
                UserImage image = entities.UserImages.Find(updatedImage.ImageID);
                image.ImageID = updatedImage.ImageID;
                image.Title = updatedImage.Title;
                image.State = updatedImage.State;
                image.DateTaken = updatedImage.DateTaken;
                image.ImageTags = updatedImage.ImageTags;
                image.Description = updatedImage.Description;
                entities.SaveChanges();
                res = true;
                Console.WriteLine("ImageRepository UpdateImage() success");
            }
            catch (Exception ex)
            {
                res = false;
                Console.WriteLine("ImageRepository UpdateImage(): " + ex.Message);
            }
            return res;
        }
        // Remove all tags associated with specified image
        public static bool RemoveOldTags(UserImage image)
        {
            bool res = false;
            try
            {
                var tags = GetImageTags();
                foreach (var tag in tags)
                {
                    if (tag.UserImageID == image.ImageID)
                    {
                        entities.ImageTags.Remove(tag);
                    }
                }
                entities.SaveChanges();
                res = true;
                Console.WriteLine("ImageRepository RemoveOldTags() success");
            }
            catch (Exception ex)
            {
                res = false;
                Console.WriteLine("ImageRepository RemoveOldTags(): " + ex.Message);
            }
            return res;
        }
        // Get list of all imae tags
        private static List<ImageTag> GetImageTags()
        {
            return entities.ImageTags.ToList();
        }
    }
}