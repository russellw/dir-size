﻿var options = true;
var names = new List<string>();
foreach (var arg in args) {
	var s = arg;
	if (options) {
		if (s == "--") {
			options = false;
			continue;
		}
		if (s.StartsWith("-")) {
			if (s.StartsWith("--"))
				s = s[1..];
			switch (s) {
			case "-?":
			case "-h":
			case "-help":
				Console.WriteLine("Usage: dir-size path...");
				return 0;
			case "-V":
			case "-v":
			case "-version":
				Console.WriteLine("dir-size 1.1");
				return 0;
			default:
				Console.WriteLine("{0}: unknown option", arg);
				return 1;
			}
		}
	}
	names.Add(s);
}
if (names.Count == 0)
	foreach (var entry in Directory.GetFileSystemEntries("."))
		names.Add(Path.GetFileName(entry));

var items = new List<Item>();
foreach (var name in names)
	items.Add(new Item(name));

items.Sort();

foreach (var item in items)
	Console.WriteLine("{0}\t{1}", item.size, item.name);
return 0;

class Item: IComparable<Item> {
	public readonly string name;
	public readonly long size;

	public Item(string name) {
		this.name = name;
		try {
			if (Directory.Exists(name))
				size = TotalSize(new DirectoryInfo(name));
			else
				size = new FileInfo(name).Length;
		} catch (UnauthorizedAccessException) {
		}
	}

	public int CompareTo(Item? other) {
		if (other == null)
			return 1;
		return size.CompareTo(other.size);
	}

	static long TotalSize(DirectoryInfo directory) {
		long total = 0;
		foreach (var entry in directory.EnumerateFileSystemInfos())
			try {
				if (entry is DirectoryInfo d) {
					total += TotalSize(d);
					continue;
				}
				total += ((FileInfo)entry).Length;
			} catch (UnauthorizedAccessException) {
			}
		return total;
	}
}
