import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'The dating app';
  users: any;

  constructor(private http: HttpClient) {

  }

  ngOnInit()
  {
    this.getUsers();
  }


  //getUsers()
  //{
  //  this.http.get("https://localhost:7194/api/users").subscribe(response => {
  //    this.users = response;
  //  }, error => {
  //    console.log(error);
  //  });
  //}

  getUsers() {
    this.http.get('https://localhost:7194/api/users').subscribe({
      next: response => this.users = response,
      error: error => console.log(error)
    })
  }
  
}
