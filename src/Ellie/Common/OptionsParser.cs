﻿#nullable disable
using CommandLine;

namespace Ellie.Common;

public static class OptionsParser
{
    public static T ParseFrom<T>(string[] args)
        where T : IEllieCommandOptions, new()
        => ParseFrom(new T(), args).Item1;

    public static (T, bool) ParseFrom<T>(T options, string[] args)
        where T : IEllieCommandOptions
    {
        using var p = new Parser(x =>
        {
            x.HelpWriter = null;
        });
        var res = p.ParseArguments<T>(args);
        var output = res.MapResult(x => x, _ => options);
        output.NormalizeOptions();
        return (output, res.Tag == ParserResultType.Parsed);
    }
}