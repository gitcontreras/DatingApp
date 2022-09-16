import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from '../../_models/members';
import { User } from '../../_models/user';
import { AccountService } from '../../_services/account.service';
import { MembersService } from '../../_services/members.service';


@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm;
  member: Member;
  currentUser: User;
  @HostListener('window:beforeunload', ['$event']) unloadNotification ($event: any)
  {
    if(this.editForm.dirty)
    {
      $event.returnValue = true;
    }
  };


  constructor(private accountService: AccountService, private memberService: MembersService, public toastr:ToastrService) {

    this.accountService.currentUser$.pipe(take(1)).subscribe(user =>
      this.currentUser = user);

  }

  ngOnInit(): void {
    this.loadMember();

  }

  loadMember() {
   
    if (this.currentUser) {
      console.log(this.currentUser);
      this.memberService.getMember(this.currentUser.userName).subscribe(member => {
        this.member = member;
      });
    }
  }

  updateMember()
  {
    this.memberService.updateMember(this.member).subscribe(() => {
      this.toastr.success('Profile updated succesfully');
      this.editForm.reset(this.member);
    });
    
    console.log(this.member);

  }

}
