﻿using DadJokesApp.Models.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DadJokesApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JokePage : ContentPage
    {
        public JokePage()
        {
            InitializeComponent();
        }

        private async void JokeButton_Clicked(object sender, EventArgs e)
        {
           DadJoke joke = await GetRemoteJoke();

           BindingContext = joke;

           DadJokeDatabase db = DadJokeDatabase.Instance;

            DadJokesApp.Models.DadJoke dbJoke = new Models.DadJoke();
            dbJoke.JokeDate = DateTime.Now;
            dbJoke.Joke = joke.joke;
            db.SaveDadJoke(dbJoke);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            DadJoke joke = await GetRemoteJoke();

            BindingContext = joke;


            DadJokeDatabase db = DadJokeDatabase.Instance;

            DadJokesApp.Models.DadJoke dbJoke = new Models.DadJoke();
            dbJoke.JokeDate = DateTime.Now;
            dbJoke.Joke = joke.joke;
            db.SaveDadJoke(dbJoke);
        }

        private async Task<DadJoke> GetRemoteJoke()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");


            string response = await client.GetStringAsync("https://icanhazdadjoke.com/");

           DadJoke joke =  JsonConvert.DeserializeObject<DadJoke>(response);

            return joke;
        }
    }
}