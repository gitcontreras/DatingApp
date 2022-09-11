import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { Member } from '../../_models/members';
import { MembersService } from '../../_services/members.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  member: Member;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

  constructor(private memberService: MembersService, private route: ActivatedRoute)
  {

  }

  ngOnInit(): void {
    this.loadMember();

    //this.galleryOptions = [
    //  {
    //    width: '500px',
    //    height: '500px',
    //    imagePercent: 100,
    //    thumbnailsColumns: 4,
    //    imageAnimation: NgxGalleryAnimation.Slide,
    //    preview:false
    //  }];

    this.galleryOptions = [
      {
        width: '600px',
        height: '400px',
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide
      },
      // max-width 800
      {
        breakpoint: 800,
        width: '100%',
        height: '600px',
        imagePercent: 80,
        thumbnailsPercent: 20,
        thumbnailsMargin: 20,
        thumbnailMargin: 20
      },
      // max-width 400
      {
        breakpoint: 400,
        preview: false
      }
    ];

    this.galleryImages = [
      {
        small: './assets/user.png',
        medium: './assets/user.png',
        big: './assets/user.png'
      },
      {
        small: 'https://randomuser.me/api/portraits/men/90.jpg',
        medium: 'https://randomuser.me/api/portraits/men/90.jpg',
        big: 'https://randomuser.me/api/portraits/men/90.jpg'
      },
      {
        small: 'https://randomuser.me/api/portraits/men/90.jpg',
        medium: 'https://randomuser.me/api/portraits/men/90.jpg',
        big: 'https://randomuser.me/api/portraits/men/90.jpg'
      }
    ];
  }

  getImages(): NgxGalleryImage []
  {
    const imageUrls = [];
    if (this.member)
    {
      for (const photo of this.member.photos) {
        imageUrls.push({
          small: photo?.url,
          medium: photo?.url,
          big: photo?.url
        })
      }
    }

    return imageUrls;
  }


      

  loadMember() {
    this.memberService.getMember(this.route.snapshot.paramMap.get('username')).subscribe(member => {
      this.member = member;
     // this.galleryImages = this.getImages();
  });
  }

}
