import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { AuthGuard } from './auth.guard';
import { AuthService } from './shared/auth.service';
import { of } from 'rxjs';

describe('AuthGuard', () => {
  let guard: AuthGuard;
  let authService: AuthService;
  let router: Router;

  beforeEach(() => {
    const authServiceMock = {
      isLoggedIn: jasmine.createSpy('isLoggedIn').and.returnValue(false) // Kullanıcı giriş yapmamış olarak varsayıyoruz
    };
    
    const routerMock = {
      navigate: jasmine.createSpy('navigate')
    };

    TestBed.configureTestingModule({
      providers: [
        AuthGuard,
        { provide: AuthService, useValue: authServiceMock },
        { provide: Router, useValue: routerMock }
      ]
    });

    guard = TestBed.inject(AuthGuard);
    authService = TestBed.inject(AuthService);
    router = TestBed.inject(Router);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });

  it('should navigate to login if not logged in', () => {
    // Setup AuthService to return false
    (authService.isLoggedIn as jasmine.Spy).and.returnValue(false);
    
    const result = guard.canActivate();
    
    expect(result).toBeFalse();
    expect(router.navigate).toHaveBeenCalledWith(['/login']);
  });

  it('should allow access if logged in', () => {
    // Setup AuthService to return true
    (authService.isLoggedIn as jasmine.Spy).and.returnValue(true);
    
    const result = guard.canActivate();
    
    expect(result).toBeTrue();
    expect(router.navigate).not.toHaveBeenCalled();
  });
});
