﻿namespace G24W1502RestAPI;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;

public class GameViewModel : INotifyPropertyChanged
{
    private ObservableCollection<Game> _games
        = new ObservableCollection<Game>();

    public ObservableCollection<Game> Games => _games;

    private Game? _selectedGame = null;
    
    public Game? SelectedGame
    {
        get => _selectedGame;
        set
        {
            if (value == null || _selectedGame == value)
                return;

            _selectedGame = value;
            OnPropertyChanged(nameof(SelectedGame));
            OnPropertyChanged(nameof(IsSelected));
        }
    }

    //클릭할 때 사진이 나오도록 함. null이면 숨긴다.
    public Visibility IsSelected => (_selectedGame == null)
            ? Visibility.Hidden
            : Visibility.Visible;

    //---------------------------------------------

    // 참고
    // https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient
    // REST API 접속을 위한 변수
    private static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("https://www.freetogame.com/api/"),
    };

    public async Task GetGames()
    {
        try
        {
            // https://www.freetogame.com/api/games?platform=pc 에서 JSON 받아옴
            HttpResponseMessage response
                    = await sharedClient.GetAsync("games?platform=pc");
            response.EnsureSuccessStatusCode();

            string result = await response.Content.ReadAsStringAsync();
            // 디버깅을 위해 출력 창에 결과 출력
            //System.Diagnostics.Debug.WriteLine(result);
            if (string.IsNullOrEmpty(result)) return;

            // 서버에서 받은 JSON을 List로 변환
            List<Game>? resultGames
                = JsonSerializer.Deserialize<List<Game>>(result);
            if (resultGames == null) return;

            // ObservableCollection에 추가
            foreach (Game game in resultGames)
            {
                _games.Add(game);
            }
        }
        catch (HttpRequestException e)
        {
            // HTTP 요청 관련 예외 처리
            System.Diagnostics.Debug.WriteLine($"Request error: {e.Message}");
        }
        catch (JsonException e)
        {
            // JSON 파싱 관련 예외 처리
            System.Diagnostics.Debug.WriteLine($"JSON parse error: {e.Message}");
        }
        catch (Exception e)
        {
            // 그 외의 예외 처리
            System.Diagnostics.Debug.WriteLine($"Unexpected error: {e.Message}");
        }
    }

    //---------------------------------------------
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
