import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { UserModule } from './user/user.module';
import { DashboardModule } from './dashboard/dashboard.module';
import { AppRoutingModule } from './app-routing.module';
import { MenuComponent } from './dashboard/menu/menu.component';    
import { AppComponent } from './app.component';
import { withFetch } from '@angular/common/http';
import { LoginComponent } from './user/login/login.component';
import { RegisterComponent } from './user/register/register.component';
import { ChatService } from './shared/chat.service';
import { ChatComponent } from './chat/chat.component';

@NgModule({
    declarations: [
        AppComponent,
        MenuComponent,
        ChatComponent
    ],
    imports: [
        BrowserModule,
        FormsModule,
        HttpClientModule,
        RouterModule,
        UserModule,
        DashboardModule,
        AppRoutingModule,
        RouterModule.forRoot([
            { path: 'login', component: LoginComponent },
            { path: 'register', component: RegisterComponent }
        ])
    ],
    providers: [ChatService],
    bootstrap: [AppComponent]
})
export class AppModule { }
