import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Member } from '../../_models/members';
import { MembersService } from '../../_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  public members$: Observable<Member[]>;


  constructor(private memberService:MembersService) { }

  ngOnInit(): void {
   this.members$= this.memberService.getMembers();
  }

  //loadMembers()
  //{
  //  this.memberService.getMembers().subscribe(members => {
  //    this.members = members;
  //  });
  //}

}
