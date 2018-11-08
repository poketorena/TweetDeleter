using System;
using System.IO;
using System.Linq;
using CoreTweet;
using CsvHelper;

namespace TweetDeleter
{
    public static class Program
    {
        private static void Main()
        {
            // トークンの作成
            var tokens = Tokens.Create(
                "ConsumerKey",
                "ConsumerSecret",
                "AccessToken",
                "AccessTokenSecret"
                );

            // ファイルが存在するか確認する
            if (!File.Exists("tweets.csv"))
            {
                Console.WriteLine("tweets.csvが存在しません。");
            }

            // tweets.csvは全ツイート履歴の取得で手に入れたものを使う
            using (var streamReader = new StreamReader("tweets.csv"))
            {
                // CSVファイルを開く
                var csv = new CsvReader(streamReader);

                // CSVファイルをマッピングする
                csv.Configuration.RegisterClassMap<MyClassMap>();

                // CSVファイルから読み込み
                var records = csv.GetRecords<MyStatus>();

                // 削除ワードを含むツイートを絞り込む（リツイートを除く）
                var results = records
                    .Where(x => (x.Text.Contains("削除ワード1") || x.Text.Contains("削除ワード2")) && !x.Text.Contains("RT"));

                // 該当したIdとツイートを表示
                foreach (var item in results)
                {
                    Console.WriteLine($"{item.Id} {item.Text}");
                }

                Console.WriteLine("以上のツイートを削除しますか？（y/n）");

                var word = char.Parse(Console.ReadLine());

                if (word != 'y')
                {
                    Console.WriteLine("削除を中止しました。");
                    return;
                }

                // ツイートの削除、例外発生時はId、ツイート、例外の内容を表示する
                foreach (var item in results)
                {
                    try
                    {
                        tokens.Statuses.Destroy(id => item.Id);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"{item.Id} {item.Text}");
                        Console.WriteLine(e);
                    }
                }
                Console.WriteLine("ツイートの削除に成功しました。");
                Console.ReadLine();
            }
        }
    }
}
