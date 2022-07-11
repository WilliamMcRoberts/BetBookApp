﻿
using BetBookDbAccess;
using BetBookData.Models;
using BetBookData.Interfaces;

namespace BetBookData.Data;
public class ParleyBetData : IParleyBetData
{
    private readonly ISqlConnection _db;

    public ParleyBetData(ISqlConnection db)
    {
        _db = db;
    }

    /// <summary>
    /// Async method calls the spParleyBets_GetAll stored procedure to retrieve 
    /// all parley bets in the database
    /// </summary>
    /// <returns>IEnumerable of ParleyBetModel represents all parley bets in the database</returns>
    public async Task<IEnumerable<ParleyBetModel>> GetParleyBets()
    {
        return await _db.LoadData<ParleyBetModel, dynamic>(
        "dbo.spParleyBets_GetAll", new { });
    }

    /// <summary>
    /// Async method calls spParleyBets_Get stored procedure which retrieves one 
    /// parley bet by parley bet id
    /// </summary>
    /// <param name="parleyBetId">int represents the id of the bet being retrieved from the database</param>
    /// <returns>ParleyBetModel represents the parley bet being retrieved from the database</returns>
    public async Task<ParleyBetModel?> GetParleyBet(int parleyBetId)
    {
        var result = await _db.LoadData<ParleyBetModel, dynamic>(
            "dbo.spParleyBets_Get", new
            {
                Id = parleyBetId
            });

        return result.FirstOrDefault();
    }



    /// <summary>
    /// Async method calls the spParleyBets_Insert stored procedure to insert one parley bet 
    /// entry into the database
    /// </summary>
    /// <param name="parleyBet">ParleyBetModel represents a parley bet to insert into the database</param>
    /// <returns></returns>
    public async Task InsertParleyBet(ParleyBetModel parleyBet)
    {
        int bet1Id = parleyBet.Bets[0].Id;
        int bet2Id = parleyBet.Bets[1].Id;
        int bet3Id = 0;
        int bet4Id = 0;
        int bet5Id = 0;

        if (parleyBet.Bets.Count > 2)
        {
            bet3Id = parleyBet.Bets[2].Id;
        }

        if (parleyBet.Bets.Count > 3)
        {
            bet4Id = parleyBet.Bets[3].Id;
        }

        if (parleyBet.Bets.Count > 4)
        {
            bet5Id = parleyBet.Bets[4].Id;
        }


        string parleyBetStatus = ParleyBetStatus.IN_PROGRESS.ToString();
        string parleyPayoutStatus = ParleyPayoutStatus.UNPAID.ToString();

        await _db.SaveData("dbo.spParleyBets_Insert", new
        {
            bet1Id,
            bet2Id,
            bet3Id,
            bet4Id,
            bet5Id,
            parleyBet.BettorId,
            parleyBet.BetAmount,
            parleyBet.BetPayout,
            parleyBetStatus,
            parleyPayoutStatus
        });
    }

    /// <summary>
    /// Async method calls the spParleyBets_Update stored procedure to update
    /// a parley bet
    /// </summary>
    /// <param name="parleyBet">ParleyBetModel represents a parley bet being updated into the database</param>
    /// <returns></returns>
    public async Task UpdateParleyBet(ParleyBetModel parleyBet)
    {
        int bet1Id = parleyBet.Bets[0].Id;
        int bet2Id = parleyBet.Bets[1].Id;
        int bet3Id = 0;
        int bet4Id = 0;
        int bet5Id = 0;

        if (parleyBet.Bets.Count > 2)
        {
            bet3Id = parleyBet.Bets[2].Id;
        }

        if (parleyBet.Bets.Count > 3)
        {
            bet4Id = parleyBet.Bets[3].Id;
        }

        if (parleyBet.Bets.Count > 4)
        {
            bet5Id = parleyBet.Bets[4].Id;
        }
        string parleyBetStatus = parleyBet.ParleyBetStatus.ToString();
        string parleyPayoutStatus = parleyBet.ParleyPayoutStatus.ToString();

        await _db.SaveData("dbo.spParleyBets_Update", new
        {
            parleyBet.Id,
            bet1Id,
            bet2Id,
            bet3Id,
            bet4Id,
            bet5Id,
            parleyBet.BettorId,
            parleyBet.BetAmount,
            parleyBet.BetPayout,
            parleyBetStatus,
            parleyPayoutStatus
        });
    }

    /// <summary>
    /// Async method calls the spBets_Delete stored procedure which deletes one bet
    /// entry in the database
    /// </summary>
    /// <param name="id">int represents the id of the bet to be deleted from the database</param>
    /// <returns></returns>
    public async Task DeleteParleyBet(int id)
    {
        await _db.SaveData(
        "dbo.spBets_Delete", new
        {
            Id = id
        });
    }
}