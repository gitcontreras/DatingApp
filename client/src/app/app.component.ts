import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'The dating app';
  users: any;

  constructor(private accountService: AccountService) {

  }

  ngOnInit()
  {
    this.setCurrentUser();
  }


  //getUsers()
  //{
  //  this.http.get("https://localhost:7194/api/users").subscribe(response => {
  //    this.users = response;
  //  }, error => {
  //    console.log(error);
  //  });
  //}

  setCurrentUser()
  {
    const user: User = JSON.parse(localStorage.getItem('user'));
    this.accountService.setCurrentUser(user);
  }

  //getUsers() {
  //  this.http.get('https://localhost:7194/api/users').subscribe({
  //    next: response => this.users = response,
  //    error: error => console.log(error)
  //  })
  //}
  
}
