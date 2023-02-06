﻿namespace EurovisionApp.Common.Logic.Data.Models;

public abstract class Contestant : IEquatable<Contestant>
{
    public int Id { get; set; }
    public string Artist { get; set; }
    public string Song { get; set; }
    public IReadOnlyList<Lyrics> Lyrics { get; set; }
    public IReadOnlyList<string> VideoUrls { get; set; }
    public IReadOnlyList<string> Dancers { get; set; }
    public IReadOnlyList<string> Backings { get; set; }
    public IReadOnlyList<string> Composers { get; set; }
    public IReadOnlyList<string> Lyricists { get; set; }
    public IReadOnlyList<string> Writers { get; set; }
    public string Conductor { get; set; }
    public string StageDirector { get; set; }

    public virtual bool Equals(Contestant other)
    {
        return ReferenceEquals(this, other)
            || (other != null
            && Artist.Equals(other.Artist, StringComparison.OrdinalIgnoreCase)
            && Song.Equals(other.Song, StringComparison.OrdinalIgnoreCase));
    }
}
