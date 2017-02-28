using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using VkNet.Model;

namespace VkClient
{
    class SavedPhotoVm : VmBase
    {

        private long _offset = 0;

        private List<SavedPhoto> photoList = new List<SavedPhoto>();

        private int _currentPhotoId = 0;

        private long _userid = 17204132;

        public long UserId
        {
            get { return _userid; }
            set { Set(ref _userid, value); }
        }

        private BitmapImage _photo;

        public BitmapImage Photo
        {
            get { return _photo; }
            set { Set(ref _photo, value); }
        }

        public ICommand LoadPhotos => new CommandBase(Load);
        public ICommand PrevPhoto => new CommandBase(Prev);
        public ICommand NextPhoto => new CommandBase(Next);

        private void Prev(object parameter)
        {
            if (_currentPhotoId - 1 >= 0)
            {
                _currentPhotoId--;
                Photo = new BitmapImage(photoList[_currentPhotoId].Link);
            }
        }

        private void Next(object parameter)
        {
            if (_currentPhotoId + 1 < photoList.Count)
            {
                _currentPhotoId++;
                Photo = new BitmapImage(photoList[_currentPhotoId].Link);
            }   
    }


        private void Load(object parameter) // Load of saved photos  
        {
            var details = new VkNet.Model.RequestParams.PhotoGetParams
            {
                Extended = true,
                AlbumId = VkNet.Enums.SafetyEnums.PhotoAlbumType.Saved,
                Count = 50,
                Reversed = true,
                OwnerId = _userid,
                Offset = (ulong) _offset
            };
            var collection = api.Photo.Get(details); // getting all pics
          //  bool quality = true;
            /*
            if (Sd.IsChecked == true)
                quality = true;
            if (Hd.IsChecked == true)
                quality = false;
            foreach (var elm in collection)
            {
                switch (quality)
                {
                    case (true):
                        pics.Add(new SavedPhoto(elm.Photo130, Convert.ToString(elm.CreateTime), elm.Likes.Count, (long)elm.Id));
                        break;
                    case (false):
                        pics.Add(new SavedPhoto(elm.Photo604, Convert.ToString(elm.CreateTime), elm.Likes.Count, (long)elm.Id));
                        break;
                }
            }
            */
            foreach (var elm in collection)
                photoList.Add(new SavedPhoto(elm.Photo604, Convert.ToString(elm.CreateTime), elm.Likes.Count, (long) elm.Id));
            _offset += 50; // changing offset for next loads
            if (photoList.Count!=0)
               Photo = new BitmapImage(photoList[0].Link);
            // Likes.Text = Convert.ToString(pics[0].Likes);
            //Date.Text = pics[0].Date;
            //current_photo.Text = (curr_index + 1) + "/" + pics.Count; //pics counter
        }

       

      
    }
}
