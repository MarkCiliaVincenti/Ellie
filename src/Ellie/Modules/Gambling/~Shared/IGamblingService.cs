﻿#nullable disable
using Ellie.Econ.Gambling;
using Ellie.Econ.Gambling.Betdraw;
using Ellie.Econ.Gambling.Rps;
using OneOf;

namespace Ellie.Modules.Gambling;

public interface IGamblingService
{
    Task<OneOf<LuLaResult, GamblingError>> LulaAsync(ulong userId, long amount);
    Task<OneOf<BetrollResult, GamblingError>> BetRollAsync(ulong userId, long amount);
    Task<OneOf<BetflipResult, GamblingError>> BetFlipAsync(ulong userId, long amount, byte guess);
    Task<OneOf<SlotResult, GamblingError>> SlotAsync(ulong userId, long amount);
    Task<FlipResult[]> FlipAsync(int count);
    Task<OneOf<RpsResult, GamblingError>> RpsAsync(ulong userId, long amount, byte pick);
    Task<OneOf<BetdrawResult, GamblingError>> BetDrawAsync(ulong userId, long amount, byte? guessValue, byte? guessColor);
}