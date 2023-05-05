using AutoMapper;
using innogotchi_api.Dtos;
using innogotchi_api.Interfaces;
using innogotchi_api.Models;
using System.Collections;
using System.Net.Mime;

namespace innogotchi_api.Adapters
{
    public class AvatarAdapter : IAvatarAdapter
    {
        private readonly IAvatarRepository _avatarRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AvatarAdapter(IAvatarRepository avatarRepository, IUserRepository userRepository, IMapper mapper)
        {
            _avatarRepository = avatarRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public AvatarIdDto AddAvatar(IFormFile image)
        {
            byte[] imageBinary;

            using (var memoryStream = new MemoryStream())
            {
                image.CopyTo(memoryStream);
                imageBinary = memoryStream.ToArray();
            }

            AvatarIdDto avatar = _mapper.Map<AvatarIdDto>(_avatarRepository.AddAvatar(imageBinary));

            return avatar;
        }

        public bool AvatarExists(int id)
        {
            return _avatarRepository.AvatarExists(id);
        }

        public FormFile GetAvatar(int id)
        {
            byte[] byteArray = _avatarRepository.GetAvatar(id).Image;

            using (var stream = new MemoryStream(byteArray))
            {
                var file = new FormFile(stream, 0, byteArray.Length, "avatar", "avatar.png")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/png",
                };

                System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = file.FileName
                };
                file.ContentDisposition = cd.ToString();

                return file;
            }
        }

        public ICollection<AvatarIdDto> GetAvatarIds()
        {
            return _mapper.Map<ICollection<AvatarIdDto>>(_avatarRepository.GetAvatars());
        }
    }
}
