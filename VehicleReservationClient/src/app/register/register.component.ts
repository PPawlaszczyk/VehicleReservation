import { Component, inject, OnInit, output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { TextInputComponent } from "../_forms/text-input/text-input.component";
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, TextInputComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit{
private accountService = inject(AccountService);
private fb = inject(FormBuilder);
private router = inject(Router);

  cancelRegister = output<boolean>();
  registerForm : FormGroup = new FormGroup({});
  validationErrors: string[] | undefined;
  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(){
    this.registerForm = this.fb.group({
      username: ['',Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
    });
  }

  matchValues(matchTo: string) : ValidatorFn{
    
    return (control: AbstractControl) =>  {
      return control.value ===control.parent?.get(matchTo)?.value ? null : {isMatching: true}
    }
  }

  register() {
    this.accountService.register(this.registerForm.value).subscribe({
      next: response => this.router.navigateByUrl('/reservations'),
      error: error => this.validationErrors = error
    })
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}