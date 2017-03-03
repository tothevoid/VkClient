using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.RequestParams;

namespace VkClient
{
    class SavedPhotoVm : VmBase
    {
        private long _offset = 0;
        private long _userid = 17204132;
        private int _currentPhotoId = 0;
        private int _likes;
        private string _date;
        private string _currentPhoto;
        private List<SavedPhoto> photoList = new List<SavedPhoto>();
        private BitmapImage _photo;
        private SolidColorBrush _color;

        public SolidColorBrush Color
        {
            get { return _color; }
            set { Set(ref _color, value); }
        }

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
            set { Set(ref _userid, value); }
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
        public ICommand Like => new CommandBase(SetLike);
        public ICommand Copy => new CommandBase(CopyToSavedPhotos);
        public ICommand CopyAll => new CommandBase(SaveAllPhotos);

        private void SetLike(object parameter)
        {
            try
            {
                if (photoList[_currentPhotoId].IsLiked)
                {
                    api.Likes.Delete(LikeObjectType.Photo, photoList[_currentPhotoId].Id, _userid);
                    photoList[_currentPhotoId].Likes--;
                    photoList[_currentPhotoId].IsLiked = false;
                    GetAdditionalInfo(_currentPhotoId);
                }
                else
                {
                    api.Likes.Add(new LikesAddParams
                    {
                        ItemId = photoList[_currentPhotoId].Id,
                        Type = LikeObjectType.Photo,
                        OwnerId = _userid
                    });
                    photoList[_currentPhotoId].Likes++;
                    photoList[_currentPhotoId].IsLiked = true;
                    GetAdditionalInfo(_currentPhotoId);
                }
            }
            catch
            {
                MessageBox.Show("Calm down");
            }

        }

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

        private void Load(object parameter) 
        {
            if (photoList.Count != 0)
            {
                _offset = 0;
                photoList.Clear();
                _currentPhotoId = 0;
            }
            GetSavedPhotos();
        }

        private void LoadMorePhotos(object parameter)
        {
            GetSavedPhotos();
        }

        private void GetSavedPhotos()
        {
            var details = new PhotoGetParams
            {
                Extended = true,
                AlbumId = PhotoAlbumType.Saved,
                Count = 50,
                Reversed = true,
                OwnerId = _userid,
                Offset = (ulong) _offset
            };
            var collection = api.Photo.Get(details); 
            foreach (var elm in collection)
            { 
                bool islike = elm.Likes.UserLikes;
                photoList.Add(new SavedPhoto(QualityControl(new [] {elm.Photo75,elm.Photo130,elm.Photo604,elm.Photo807,elm.Photo1280,elm.Photo2560}), Convert.ToString(elm.CreateTime), elm.Likes.Count, (long) elm.Id, islike));
            }
            _offset += 50; // changing offset for next loads
            if (photoList.Count <= 50)
            {
                Photo = new BitmapImage(photoList[0].Link);
                GetAdditionalInfo(_currentPhotoId);
            }
            else
            {
                CurrentPhoto = $"{_currentPhotoId + 1} / {photoList.Count}";
            }
        }

        private Uri QualityControl(Uri[] Array)
        {
            for (byte i = 0; i <= 5; i++)
            {
                if (Array[i] == null)
                    return Array[i - 1];
                if (Array[i] != null && i == 5)
                    return Array[i];

            }
            throw new Exception();
        }

        private void GetAdditionalInfo(int index)
        {
            Likes = photoList[index].Likes;
            Date = photoList[index].Date;
            CurrentPhoto = $"{index + 1} / {photoList.Count}";
            Color = photoList[index].IsLiked
                ? new SolidColorBrush(Colors.CornflowerBlue)
                : new SolidColorBrush(Colors.White);
        }

        private void CopyToSavedPhotos(object parameter)
        {
            if (_userid == api.UserId)
            {
                MessageBox.Show("You're trying to save your own photo");
                return;
            }
            api.Photo.Copy(_userid, (ulong) photoList[_currentPhotoId].Id);
        }

        private async void SaveAllPhotos(object parameter)
        {
            int count = 0;
            if (photoList.Count == 0)
                return;
            foreach (var x in photoList)
            {
                if (count%3 == 0)
                {
                   // value = count/photoList.Count;
                    await Task.Delay(2000);
                }
                    
                try
                {
                    api.Photo.Copy(_userid, (ulong)x.Id);
                    count++;
                }
                catch
                {
                    MessageBox.Show("Too fast");
                    return;
                }
            }
            MessageBox.Show("Done");
        }
}
}
