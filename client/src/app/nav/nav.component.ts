import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any={ };

  constructor(public accountService: AccountService, private router:Router, private toatr:ToastrService)
  {

  }

  ngOnInit(): void {

  }


  login()
  {
    this.accountService.login(this.model).subscribe(response => {
      console.log(response);
      this.router.navigateByUrl("/members");
    });
    //console.log(this.model);
  }


  logout() {
    this.accountService.logout();
    this.router.navigateByUrl("/");
  }


}
