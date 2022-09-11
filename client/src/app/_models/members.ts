import { Photo } from "./photo";


  export interface Member {
    userName: string;
    gender: string;
    dateOfBirth: string;
    knownAs: string;
    photoUrl: string;
    created: Date;
    lastActive: Date;
    introduction: string;
    lookingFor: string;
    interests: string;
    city: string;
    country: string;
    photos: Photo[];
  }


