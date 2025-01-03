import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{
  title = 'Dating App';
  users: any;

  ngOnInit(): void {
   this.http.get('https://localhost:5001/api/users').subscribe({ //making the http request to the api , subscribing to the observable (should always unsubscribe but not in this case hhtpclient does it for us)
    next: response => this.users = response,
    error: error => console.log(error),
    complete: () => console.log('Request Completed')
   }); 
  }
  http = inject(HttpClient); // Inject the HttpClient instead of class constructor
}