namespace server.Dto
{
    public class ImageDtoRes
    {
        private string _imageUrl;
        public int Id { get; set; }
        public string ImageUrl
        {
            get { return Path.Combine("image" , _imageUrl); }
            set { _imageUrl = value; }
        }
    }

}
