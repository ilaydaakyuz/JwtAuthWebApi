import { Component } from '@angular/core';
import { UserService } from '../../shared/user.service';
import { FormsModule, NgForm } from '@angular/forms';
import { response } from 'express';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.css'
})
export class AdminDashboardComponent {
  users: any[] = [];
  user: any = {};
  selectedUserId: string | null = null;
  errorMessage: string | null = null;
  successMessage: string | null = null;

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.loadUsers();
  }
  loadUsers() {
    this.userService.getAllUsers().subscribe(
      data=>{this.users=data;}
    ),
    (error: any)=>{
      this.errorMessage='Failed to load users';
      console.error(error);
    }
  }

  createUser(form: NgForm): void {
    this.userService.createUser(this.user)
      .subscribe(
        (response) => {
          this.successMessage = 'User created successfully.';
          this.loadUsers();
          form.reset();
        },
        (error) => {
          this.errorMessage = 'Failed to create user.'
        }
      );
  }
  updateUser(form: NgForm): void {
    if (this.selectedUserId) {
      this.userService.updateUser(this.selectedUserId, this.user)
        .subscribe(
          (response) => {
            this.successMessage = 'User updated successfully.';
            this.loadUsers();
            form.reset();
            this.selectedUserId = null;
          },
          (error) => {
            this.errorMessage = 'Failed to update user.';
          }
        );
    }
  }
  deleteUser(userId: string): void {
    if (userId) {
      this.userService.deleteUser(userId)
        .subscribe(
          (response) => {
            this.successMessage = 'User deleted successfully.';
            this.loadUsers();
          },
          (error) => {
            this.errorMessage = 'Failed to delete user.';
            console.error(error);
          }
        );
    } else {
      this.errorMessage = 'Invalid user ID.';
    }
  }

  selectUserForUpdate(user: any): void {
    this.selectedUserId = user.userId;
    this.user = { ...user };
    console.log('Selected User ID:', this.selectedUserId);
  }
}
