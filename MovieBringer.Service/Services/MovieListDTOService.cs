 
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MovieBringer.Core.DTOs;
using MovieBringer.Core.DTOs.MovieListDTO;
using MovieBringer.Core.DTOs.UserDTO;
using MovieBringer.Core.Entities;
using MovieBringer.Core.Models.MovieModels;
using MovieBringer.Core.Repositories;
using MovieBringer.Core.Services;
using MovieBringer.Core.UnitOfWorks;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MovieBringer.Service.Services
{
    public class MovieListDTOService : DTOService<MovieList, MovieListDto>, IMovieListDTOService
    {
        private readonly IMovieListRepository _movieListRepository;
        private readonly IMovieListHistoryRepository _movieListHistoryRepository;
        public MovieListDTOService(IMapper mapper, IUnitOfWork unitOfWork, IGenericRepository<MovieList> repository, IMovieListRepository movieListRepository, IMovieListHistoryRepository movieListHistoryRepository) : base(mapper, unitOfWork, repository)
        {
            _movieListRepository = movieListRepository;
            _movieListHistoryRepository = movieListHistoryRepository;
        }


        public async Task<CustomResponseDto<List<MovieListDto>>> GetListsByUserId(string userID)
        {
            var moviLists=await _movieListRepository.GetListsByUserId(userID);
            var moviListsDto= _mapper.Map<List<MovieListDto>>(moviLists);
            return CustomResponseDto<List<MovieListDto>>.Success(200, moviListsDto);
        }

        public async Task<CustomResponseDto<int>> GetMostPopulerListId()
        {
            var moviListId = await _movieListRepository.GetMostPopulerListId();
            return CustomResponseDto<int>.Success(200, moviListId);
        }

        public async Task<CustomResponseDto<int>> GetLastAddedListId()
        {
            var moviListId = await _movieListRepository.GetLastAddedListId();
            return CustomResponseDto<int>.Success(200, moviListId);
        }

        public async Task<CustomResponseDto<List<MovieListDto>>> GetPublicLists()
        {
            var publicLists= await _movieListRepository.GetPublicLists();
            var publicListsDto=_mapper.Map<List<MovieListDto>>(publicLists);
            return CustomResponseDto<List<MovieListDto>>.Success(200, publicListsDto);
        }

        public async Task<CustomResponseDto<MovieListDto>> AddAsync(MovieListCreateDto dto)
        {
            var newEntity = _mapper.Map<MovieList>(dto);
            await _movieListRepository.AddAsync(newEntity);
            await _unitOfWork.CommitAsync();
            var newDto = _mapper.Map<MovieListDto>(newEntity);

            return CustomResponseDto<MovieListDto>.Success(StatusCodes.Status200OK, newDto);
        }

        public async Task<CustomResponseDto<NoContentDto>> UpdateAsync(MovieListUpdateDto dto)
        {
            var entity = _mapper.Map<MovieList>(dto);
            _movieListRepository.Update(entity);
            await _unitOfWork.CommitAsync();

            return CustomResponseDto<NoContentDto>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<CustomResponseDto<NoContentDto>> UpdateAsync(MovieList movieList)
        {
            _movieListRepository.Update(movieList);
            await _unitOfWork.CommitAsync();

            return CustomResponseDto<NoContentDto>.Success(StatusCodes.Status204NoContent);
        }


        public async Task<CustomResponseDto<MovieListHistory>> CreateMovieListHistory(MovieListHistoryCreateModel movieListHistoryModel)
        {
            var movieListHisModel = _mapper.Map<MovieListHistory>(movieListHistoryModel);
            //ilk dispay order atıyoruz
            movieListHisModel.DisplayOrder = await _movieListHistoryRepository.SetFirsDisplayOrder(movieListHistoryModel.MovieListId);
            var movieListHist = await _movieListHistoryRepository.AddAsync(movieListHisModel);
            await _unitOfWork.CommitAsync();
            return CustomResponseDto<MovieListHistory>.Success(200, movieListHist, "MovieHistoryList successfully created");
        }

        public async Task<CustomResponseDto<NoContentDto>> DeleteMovieList(int listId)
        {
            await _movieListHistoryRepository.DeleteList(listId);
            await _unitOfWork.CommitAsync();
            return CustomResponseDto<NoContentDto>.Success(200, "List successfully deleted");
        }

        public async Task<CustomResponseDto<NoContentDto>> RemoveMovieFromList(int movieHistoryId)
        {
            var movieListHistory = await _movieListHistoryRepository.GetByIdAsync(movieHistoryId);

            if (movieListHistory != null)
            {
                _movieListHistoryRepository.Remove(movieListHistory);
                await _unitOfWork.CommitAsync();
                return CustomResponseDto<NoContentDto>.Success(200, "Movie removed from list successfully");
            }

            return CustomResponseDto<NoContentDto>.Fail(500,"Error Happened");
        }

        public async Task<CustomResponseDto<NoContentDto>> SyncDisplayOrder(int listId)
        {
            await _movieListHistoryRepository.SyncDisplayOrder(listId);
            return CustomResponseDto<NoContentDto>.Success(200, "Synced successfully");
        }

        public async Task<CustomResponseDto<NoContentDto>> MovieOrderUp(int movieListHistoryId,int listId)
        {
            var movieHistory = await _movieListHistoryRepository.GetByIdAsync(movieListHistoryId);

            if (movieHistory != null && movieHistory.DisplayOrder != 1)
            {
                //cliente uygun hale getirilcek
                var forwardHistory = await _movieListHistoryRepository.FindByDisplayOrder(movieHistory.DisplayOrder - 1,listId);
                forwardHistory.DisplayOrder++;
                _movieListHistoryRepository.Update(forwardHistory);

                movieHistory.DisplayOrder--;
                _movieListHistoryRepository.Update(movieHistory);
                await _unitOfWork.CommitAsync();
                return CustomResponseDto<NoContentDto>.Success(200, "Ordered Successfuly");
            }
            return CustomResponseDto<NoContentDto>.Fail(500, "already first place");
        }

        public async Task<CustomResponseDto<NoContentDto>> MovieOrderDown(int movieListHistoryId, int listId)
        {
            var movieHistory =  await _movieListHistoryRepository.GetByIdAsync(movieListHistoryId);

            var historiesCount = await _movieListHistoryRepository.GetHistoryCount(movieHistory.MovieListId);

            if (movieHistory != null && movieHistory.DisplayOrder != historiesCount)
            {
                var backwardHistory = await _movieListHistoryRepository.FindByDisplayOrder(movieHistory.DisplayOrder + 1,listId);

                backwardHistory.DisplayOrder--;
                 _movieListHistoryRepository.Update(backwardHistory);

                movieHistory.DisplayOrder++;
                _movieListHistoryRepository.Update(movieHistory);

                await _unitOfWork.CommitAsync();
                return CustomResponseDto<NoContentDto>.Success(200, "Ordered Successfuly");

            }
            return CustomResponseDto<NoContentDto>.Fail(500, "already last place");
        }

        public async Task<CustomResponseDto<NoContentDto>> VoteMovieList(int voteValue, int listId, string voteOwnerId)
        {
            var isListVoted = await _movieListRepository.IsListVoted(voteOwnerId, listId);

            if (isListVoted)
            {
                await _movieListRepository.UpdateVoteListMovie(voteValue, listId, voteOwnerId);
                await _unitOfWork.CommitAsync();
                return CustomResponseDto<NoContentDto>.Success(400, "your vote updated successfuly");
            }
            else
            {

                await _movieListRepository.CreteVoteListMovie(voteValue, listId, voteOwnerId);
                await _unitOfWork.CommitAsync();

                return CustomResponseDto<NoContentDto>.Success(200, "Voted Successfuly");
            }
                
        }

        public async Task<CustomResponseDto<VoteMovieList>> CheckMovieListVotable(int voteValue, int listId, string voteOwnerId)
        {
            var isListVoted = await _movieListRepository.IsListVoted(voteOwnerId, listId);

            if (isListVoted)
            {
                var voteMovieList = await _movieListRepository.GetVoteMovieList(voteOwnerId, listId);
                return CustomResponseDto<VoteMovieList>.Success(400, voteMovieList,"list voted already");
            }
            else
            {
                return CustomResponseDto<VoteMovieList>.Fail(400, "List Votable");
            }
                
        }

        public async Task<CustomResponseDto<double>> GetMovieListVote(int listId)
        {
            var rankMovieList = await _movieListRepository.GetRankMovieList(listId);

            if(rankMovieList!=0)
                return CustomResponseDto<double>.Success(400, rankMovieList);
            else
                return CustomResponseDto<double>.Success(400, 0);

        }


    }
}
