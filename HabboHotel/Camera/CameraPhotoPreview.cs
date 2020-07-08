namespace Neon.HabboHotel.Camera
{
    public class CameraPhotoPreview
    {
        private readonly int _photoId;
        private readonly int _creatorId;
        private readonly long _createdAt;

        public int Id => _photoId;

        public int CreatorId => _creatorId;

        public long CreatedAt => _createdAt;

        public CameraPhotoPreview(int photoId, int creatorId, long createdAt)
        {
            _photoId = photoId;
            _creatorId = creatorId;
            _createdAt = createdAt;
        }
    }
}