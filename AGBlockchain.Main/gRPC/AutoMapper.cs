using AGBlockchain.gRPC.Server;
using AGBlockchain.Structure.Models;
using AutoMapper;

namespace AGBlockchain.Main.gRPC
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<string, string>().ConvertUsing(str => str ?? string.Empty);

            CreateMap<Transaction, TransactionModel>();//.ReverseMap();

            CreateMap<Block, BlockModel>();//.ReverseMap();

            CreateMap<SendRequest, Transaction>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TransactionId))
                .ForMember(dest => dest.Signature, opt => opt.MapFrom(src => src.TransactionInput.Signature))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.TransactionInput.Timestamp))
                .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.TransactionInput.Sender))
                .ForMember(dest => dest.Recipient, opt => opt.MapFrom(src => src.TransactionOutput.Recipient))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.TransactionOutput.Amount))
                .ForMember(dest => dest.Fee, opt => opt.MapFrom(src => src.TransactionOutput.Fee));
        }
    }
}
