﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VkNet.Enums.SafetyEnums;
using VkNet.Exception;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace VkClient
{
    class SavedPhotoVm : VmBase
    {
        // TODO: Visibility + focus
        // TODO: History 
        // UPDATE: ID field

        public SavedPhotoVm()
        {
            DateStart = DateTime.Now;
            DateEnd = DateTime.Now;
            GetIDs();
        }

        private long _offset = 0;
        private long _userid = 17204132;
        private int _currentPhotoId = 0;
        private int _cbId = -1;
        private int _likes;
        private string _date;
        private string _currentPhoto;
        private List<SavedPhoto> photoList = new List<SavedPhoto>();
        private BitmapImage _photo;
        private SolidColorBrush _color;
        private Visibility _opened = Visibility.Collapsed;
        private DateTime _dateStart;
        private DateTime _dateEnd;
        private DateTime _selectedDate;

        public ICommand LoadPhotos => new CommandBase(Load);
        public ICommand PrevPhoto => new CommandBase(Prev);
        public ICommand NextPhoto => new CommandBase(Next);
        public ICommand LoadMore => new CommandBase(LoadMorePhotos);
        public ICommand Like => new CommandBase(SetLike);
        public ICommand Copy => new CommandBase(CopyToSavedPhotos);
        public ICommand CopyAll => new CommandBase(SaveAllPhotos);
        public ICommand MyProfile => new CommandBase(LoadMyProfile);

        public ObservableCollection<string> ListOfIDs { get; set; } = new ObservableCollection<string>();

        public Visibility Opened
        {
            get { return _opened; }
            set { Set(ref _opened, value); }
        }

        public SolidColorBrush Color
        {
            get { return _color; }
            set { Set(ref _color, value); }
        }

        public int CbID
        {
            get { return _cbId; }
            set
            {
                Set(ref _cbId, value);
                UserId = Convert.ToInt64(ListOfIDs[_cbId]);
                Load(null);
            }
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

        public DateTime DateStart
        {
            get { return _dateStart; }
            set { Set(ref _dateStart, value); }
        }

        public DateTime DateEnd
        {
            get { return _dateEnd; }
            set { Set(ref _dateEnd, value); }
        }

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { Set(ref _selectedDate, value); SearchByDate(); }
        }

        private void SearchByDate()
        {
            int counter = -1;

            foreach (var x in photoList)
            {
               counter++;
               if (x.Date <= SelectedDate.AddDays(1))
               {
                    _currentPhotoId = counter;
                    Update();
                    GetAdditionalInfo(_currentPhotoId);
                    return;
               }
              
            }
        }

        public void SwitchVisibility()
        {
            Opened = Visibility.Visible;
        }

        private void GetIDs()
        {
            var IDs = System.IO.File.ReadAllLines("IDs.txt");
            foreach (var x in IDs)
            {
                ListOfIDs.Add(x);
            }
        }

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
                CustomMessageBox.Show( "Likes exception","Too many likes per minute");
            }
        }

        public void Prev(object parameter)
        {
            if (_currentPhotoId - 1 >= 0)
            {
                _currentPhotoId--;
                Photo = new BitmapImage(photoList[_currentPhotoId].Link);
                GetAdditionalInfo(_currentPhotoId);
            }
        }

        public void Next(object parameter)
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
                OwnerId = UserId,
                Offset = (ulong)_offset
            };
            VkCollection<Photo> collection;
            try
            {
                collection = api.Photo.Get(details);
                if (ListOfIDs.Contains(UserId.ToString()) == false)
                {
                    ListOfIDs.Add(UserId.ToString());
                    System.IO.File.AppendAllText("IDs.txt", _userid + Environment.NewLine);
                }
            }
            catch (Exception exception)
            {
                CustomMessageBox.Show("Loading error",exception.Message);
                return;
            }
            foreach (var elm in collection)
            {
                bool islike = elm.Likes.UserLikes;
                photoList.Add(new SavedPhoto(QualityControl(new[] { elm.Photo75, elm.Photo130, elm.Photo604, elm.Photo807, elm.Photo1280, elm.Photo2560 }), (DateTime) elm.CreateTime, elm.Likes.Count, (long)elm.Id, islike));
                //photoList.Add(new SavedPhoto(QualityControl(new[] { elm.Photo75, elm.Photo130, elm.Photo604, elm.Photo807, elm.Photo1280, elm.Photo2560 }), Convert.ToString(elm.CreateTime), elm.Likes.Count, (long)elm.Id, islike));
            }

            if (photoList.Count == 0)
            {
                CustomMessageBox.Show("Load exception", "This account has no saved photos");
                return;
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

            DateStart = photoList[photoList.Count-1].Date;
            DateEnd = photoList[0].Date;
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
            Date = Convert.ToString(photoList[index].Date);
            //Date = photoList[index].Date;
            CurrentPhoto = $"{index + 1} / {photoList.Count}";
            Color = photoList[index].IsLiked
                ? new SolidColorBrush(Colors.CornflowerBlue)
                : new SolidColorBrush(Colors.White);
        }

        private void CopyToSavedPhotos(object parameter)
        {
            if (UserId == api.UserId)
            {
                CustomMessageBox.Show("Load error","You're trying to save your own photo");
                return;
            }
            api.Photo.Copy(UserId, (ulong)photoList[_currentPhotoId].Id);
        }

        private async void SaveAllPhotos(object parameter)
        {
            MessageBoxResult messageBoxResult = CustomMessageBox.Show("Are you sure?", "Save'em all", MessageBoxType.YesNo);
            if (messageBoxResult == MessageBoxResult.No)
                return;
            int count = 0;
            if (photoList.Count == 0)
                return;
            foreach (var x in photoList)
            {
                if (count % 3 == 0)
                {
                    await Task.Delay(2000);
                }
                try
                {
                    api.Photo.Copy(UserId, (ulong)x.Id);
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

        public void Update()
        {
            Photo = new BitmapImage(photoList[_currentPhotoId].Link);
        }

        private void LoadMyProfile(object parameter)
        {
            if (api.UserId == null)
            {
                CustomMessageBox.Show("Action exception","You're not logged in");
                return;
            }
            var profile = api.Friends.Get(new FriendsGetParams { UserId=api.UserId, Count=100,Fields=VkNet.Enums.Filters.ProfileFields.All});
            UserId = (long) api.UserId;
            Load(null);
        }
    }
}
