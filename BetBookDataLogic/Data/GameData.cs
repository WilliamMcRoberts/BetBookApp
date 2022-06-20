﻿using BetBookDataLogic.Data.Interfaces;
using BetBookDataLogic.DbAccess;
using BetBookDataLogic.Models;

namespace BetBookDataLogic.Data;
public class GameData : IGameData
{

    private readonly ISqlConnection _db;

    /// <summary>
    /// GameData Constructor
    /// </summary>
    /// <param name="db"></param>
    public GameData(ISqlConnection db)
    {
        _db = db;
    }

    /// <summary>
    /// Method calls the spGames_GetAll stored procedure to retrieve 
    /// all games in the database
    /// </summary>
    /// <returns>IEnumerable of GameModel</returns>
    public async Task<IEnumerable<GameModel>> GetGames()
    {
        return await _db.LoadData<GameModel, dynamic>(
        "dbo.spGames_GetAll", new { });
    }

    /// <summary>
    /// Method calls the spGames_GetAllByWeek stored procedure to retrieve 
    /// all games in the database by a certain week
    /// </summary>
    /// <returns>IEnumerable of GameModel</returns>
    public async Task<IEnumerable<GameModel>> GetGamesByWeek(int weekNumber)
    {
        return await _db.LoadData<GameModel, dynamic>(
        "dbo.spGames_GetAllByWeek", new
        {
            WeekNumber = weekNumber
        });
    }

    /// <summary>
    /// Method calls the spGames_GetAllFinished stored procedure to retrieve 
    /// all games that have a "FINISHED" status
    /// </summary>
    /// <returns>IEnumerable of GameModel</returns>
    public async Task<IEnumerable<GameModel>> GetAllFinishedGames()
    {
        return await _db.LoadData<GameModel, dynamic>(
        "dbo.spGames_GetAllFinished", new { });
    }

    /// <summary>
    /// Method calls the spGames_GetAllInProgress stored procedure to retrieve 
    /// all games that have an "IN_PROGRESS" status
    /// </summary>
    /// <returns>IEnumerable of GameModel</returns>
    public async Task<IEnumerable<GameModel>> GetAllInProgressGames()
    {
        return await _db.LoadData<GameModel, dynamic>(
        "dbo.spGames_GetAllInProgress", new { });
    }

    /// <summary>
    /// Method calls the spGames_GetAllNotStarted stored procedure to retrieve 
    /// all games that have a "NOT_STARTED" status
    /// </summary>
    /// <returns>IEnumerable of GameModel</returns>
    public async Task<IEnumerable<GameModel>> GetAllNotStartedGames()
    {
        return await _db.LoadData<GameModel, dynamic>(
        "dbo.spGames_GetAllNotStarted", new { });
    }

    /// <summary>
    /// Method calls spGames_Get stored procedure which retrieves one 
    /// game by game id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>GameModel</returns>
    public async Task<GameModel?> GetGame(int id)
    {
        var results = await _db.LoadData<GameModel, dynamic>(
            "dbo.spGames_Get", new
            {
                Id = id
            });

        return results.FirstOrDefault();
    }

    /// <summary>
    /// Method calls the spGames_Insert stored procedure to insert one game 
    /// entry into the database
    /// </summary>
    /// <param name="game"></param>
    /// <returns></returns>
    public async Task InsertGame(GameModel game)
    {
        int weekNumber = game.Week;
        string seasonType = game.Season.ToString();
        string gameStatus = game.GameStatus.ToString();

        await _db.SaveData("dbo.spGames_Insert", new
        {
            game.HomeTeamId,
            game.AwayTeamId,
            game.FavoriteId,
            game.UnderdogId,
            game.Stadium,
            game.PointSpread,
            weekNumber,
            seasonType,
            game.DateOfGame,
            gameStatus
        });
    }

    /// <summary>
    /// Method calls the spGames_Update stored procedure to update a game
    /// </summary>
    /// <param name="game"></param>
    /// <returns></returns>
    public async Task UpdateGame(GameModel game)
    {
        string seasonType = game.Season.ToString();
        string gameStatus = game.GameStatus.ToString();

        await _db.SaveData("dbo.spGames_Update", new
        {
            game.Id,
            game.HomeTeamId,
            game.AwayTeamId,
            game.FavoriteId,
            game.UnderdogId,
            game.Stadium,
            game.PointSpread,
            game.FavoriteFinalScore,
            game.UnderdogFinalScore,
            seasonType,
            game.DateOfGame,
            gameStatus
        });
    }

    /// <summary>
    /// Method calls the spGames_AddWinner stored procedure to declare the 
    /// winner of a game
    /// </summary>
    /// <param name="game"></param>
    /// <returns></returns>
    public async Task AddGameWinner(GameModel game, TeamModel team)
    {
        int gameWinnerId = team.Id;

        await _db.SaveData("dbo.spGames_AddWinner", new
        {
            game.Id,
            gameWinnerId
        });
    }

    /// <summary>
    /// Method calls the spGames_Delete stored procedure which deletes one game
    /// entry in the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteGame(int id)
    {
        await _db.SaveData(
        "dbo.spGames_Delete", new
        {
            Id = id
        });
    }
}
