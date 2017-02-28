using System;
using System.Collections.Generic;
using System.IO.Pipes;
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
        private BitmapImage _photo;
        private int _likes;
        private string _date;
        private string _currentPhoto;

        public string CurrentPhoto
        {
            get { return _currentPhoto; }
            set { Set(ref _currentPhoto, value); }
        }

        public string Date
        {
            get { return _date; }
            set { Set(ref _date, value); }
        }

        public int Likes
        {
            get { return _likes; }
            set { Set(ref _likes, value); }
        }

        public long UserId
        {
            get { return _userid; }
            set  { Set(ref _userid, value); }
        }

        public BitmapImage Photo
        {
            get { return _photo; }
            set { Set(ref _photo, value); }
        }



        public ICommand LoadPhotos => new CommandBase(Load);
        public ICommand PrevPhoto => new CommandBase(Prev);
        public ICommand NextPhoto => new CommandBase(Next);
        public ICommand LoadMore => new CommandBase(LoadMorePhotos);



        private void Prev(object parameter)
        {
            if (_currentPhotoId - 1 >= 0)
            {
                _currentPhotoId--;
                Photo = new BitmapImage(photoList[_currentPhotoId].Link);
                GetAdditionalInfo(_currentPhotoId);
            }
        }

        private void Next(object parameter)
        {
            if (_currentPhotoId + 1 < photoList.Count)
            {
                _currentPhotoId++;
                Photo = new BitmapImage(photoList[_currentPhotoId].Link);
                GetAdditionalInfo(_currentPhotoId);
            }   
        }

        private void Load(object parameter) // Loading saved photos 
        {
           if (photoList.Count!=0)
                photoList.Clear();
           GetSavedPhotos();
        }

        private void LoadMorePhotos(object parameter)
        {
            GetSavedPhotos();
        }

        private void GetSavedPhotos()
        {
            var details = new VkNet.Model.RequestParams.PhotoGetParams
            {
                Extended = true,
                AlbumId = VkNet.Enums.SafetyEnums.PhotoAlbumType.Saved,
                Count = 50,
                Reversed = true,
                OwnerId = _userid,
                Offset = (ulong)_offset
            };
            var collection = api.Photo.Get(details); // getting all pics
            foreach (var elm in collection)
                photoList.Add(new SavedPhoto(elm.Photo604, Convert.ToString(elm.CreateTime), elm.Likes.Count, (long)elm.Id));
            _offset += 50; // changing offset for next loads
            if (photoList.Count == 50)
            {
                Photo = new BitmapImage(photoList[0].Link);
                GetAdditionalInfo(_currentPhotoId);
            }
        }

        private void GetAdditionalInfo(int index)
        {
            Likes = photoList[index].Likes;
            Date = photoList[index].Date;
            CurrentPhoto = $"{index + 1} / {photoList.Count}";
        }
      
    }
}
