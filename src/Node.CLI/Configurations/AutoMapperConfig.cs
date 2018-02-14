using AutoMapper;
using Node.CLI.Models;
using Node.Core.Models;

namespace Node.CLI.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Peer, PeerViewModel>()
                .ReverseMap();

            CreateMap<Transaction, TransactionViewModel>()
                .ReverseMap();

            CreateMap<Block, BlockViewModel>()
                .ReverseMap();
        }
    }
}