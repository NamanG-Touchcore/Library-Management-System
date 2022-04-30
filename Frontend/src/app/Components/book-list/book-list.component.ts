import { Component, Input, OnInit } from '@angular/core';
import { GlobalStoreService } from 'src/app/Services/global-store.service';

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.css']
})
export class BookListComponent  {

  constructor(private store:GlobalStoreService) { }
  @Input() sectionTitle:any;
  @Input() itemList:any=[];
  @Input() dataType:any=0;//0 book, 1 issue
  collapsed:boolean=false;
  toggleCollapse=()=>{this.collapsed=!this.collapsed}
  deleteCallback=(id:number)=>
    {
      this.itemList=this.itemList.filter((book:any)=>book.id!=id)
    }
}
