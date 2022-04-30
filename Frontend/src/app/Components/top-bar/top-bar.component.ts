import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatFormFieldControl } from '@angular/material/form-field';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { GlobalStoreService } from 'src/app/Services/global-store.service';
import { BookEditDialogComponent } from '../book-edit-dialog/book-edit-dialog.component';
@Component({
  selector: 'app-top-bar',
  templateUrl: './top-bar.component.html',
  styleUrls: ['./top-bar.component.css'],
})
export class TopBarComponent implements OnInit, OnDestroy {
  constructor(
    private route: Router,
    private service: GlobalStoreService,
    private dialog: MatDialog,
    private activatedRoute: ActivatedRoute
  ) {}
  days = [
    'Sunday',
    'Monday',
    'Tuesday',
    'Wednesday',
    'Thursday',
    'Friday',
    'Saturday',
  ];
  pageTitle: string = 'Admin Dashboard';
  systemDate: any = new Date(Date.now());
  dateStr: any;
  searchBarShown: boolean = false;
  dateObservable = new Observable((observer) => {
    let systemDate: Date = new Date(Date.now());
    let interval: any;
    observer.next(systemDate);
    interval = setInterval(() => {
      systemDate.setMinutes(systemDate.getMinutes() + 1);
      observer.next(systemDate);
    }, 60000);

    return {
      unsubscribe() {
        clearInterval(interval);
      },
    };
  });

  setDateStr = (date: any) => {
    this.dateStr = `${
      date.getHours() > 12 ? date.getHours() % 12 : date.getHours()
    }:${date.getMinutes() < 10 ? '0' + date.getMinutes() : date.getMinutes()} ${
      date.getHours() >= 12 ? 'PM' : 'AM'
    }, ${this.days[date.getDay()]}`;
  };

  buttonClick = () => {
    alert('click');
  };
  toggleSearchBar = () => {
    this.searchBarShown = !this.searchBarShown;
  };
  myControl = new FormControl();
  books: any = [];
  options: any = [];
  filteredOptions: any;
  addBook() {
    const dialogRef = this.dialog.open(BookEditDialogComponent, {
      width: '550px',
      data: { type: 0 },
    });
  }
  goHome() {
    this.route.navigateByUrl('/books');
  }
  ngOnInit(): void {
    this.filteredOptions = this.myControl.valueChanges.pipe(
      startWith(''),
      map((value) => this._filter(value))
    );
    this.dateObservable.subscribe({
      next: (val) => {
        this.systemDate = val;
        this.setDateStr(val);
      },
    });
    this.user = this.service.getUser();
    this.service.getBooks().subscribe(
      (book) =>
        (this.options = book
          .filter((b: any) => b.isBookActive == 1)
          .map((b: any) => {
            return { name: b.name, id: b.id };
          })
          .sort())
    );
  }
  user: any;
  logOut() {
    this.service.logOut();
    this.route.navigateByUrl('login');
  }
  onSubmit = () => {
    const navId = this.options.find(
      (i: any) => i.name == this.myControl.value
    ).id;
    if (navId) {
      this.route.navigate(['books', navId]);
      // this.activatedRoute.paramMap.subscribe()
      this.myControl.setValue('');
      this.searchBarShown = false;
    }
    return false;
  };
  private _filter(value: any): any[] {
    const filterValue = value.toLowerCase();

    return this.options.filter((option: any) =>
      option.name.toLowerCase().includes(filterValue)
    );
  }
  ngOnDestroy(): void {}
}
