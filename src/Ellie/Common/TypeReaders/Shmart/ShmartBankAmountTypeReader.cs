﻿#nullable disable
using Ellie.Modules.Gambling.Bank;
using Ellie.Modules.Gambling.Services;

namespace Ellie.Common.TypeReaders;

public sealed class ShmartBankAmountTypeReader : EllieTypeReader<ShmartBankAmount>
{
    private readonly IBankService _bank;
    private readonly ShmartBankInputAmountReader _tr;

    public ShmartBankAmountTypeReader(IBankService bank, DbService db, GamblingConfigService gambling)
    {
        _bank = bank;
        _tr = new ShmartBankInputAmountReader(bank, db, gambling);
    }

    public override async ValueTask<TypeReaderResult<ShmartBankAmount>> ReadAsync(ICommandContext ctx, string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return TypeReaderResult.FromError<ShmartBankAmount>(CommandError.ParseFailed, "Input is empty.");

        var result = await _tr.ReadAsync(ctx, input);

        if (result.TryPickT0(out var val, out var err))
        {
            return TypeReaderResult.FromSuccess<ShmartBankAmount>(new(val));
        }

        return TypeReaderResult.FromError<ShmartBankAmount>(CommandError.Unsuccessful, err.Value);
    }
}