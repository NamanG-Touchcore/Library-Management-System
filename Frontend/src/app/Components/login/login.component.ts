import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { GlobalStoreService } from 'src/app/Services/global-store.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  constructor(private router: Router, private service: GlobalStoreService) {}
  username = new FormControl('');
  password = new FormControl('');
  ngOnInit(): void {}
  onSubmit() {
    this.service
      .login(this.username.value, this.password.value)
      .subscribe((res: any) => {
        this.service.setUser(res.username, res.role, res.token);
        this.router.navigateByUrl('/books');
      });
    return false;
  }
}
