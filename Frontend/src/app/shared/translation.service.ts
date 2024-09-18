import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TranslationService {
  private apiKey: string = "ad08d63e-88dc-41f8-8a55-9634041f8de8";
  private apiUrl: string = "https://api-translate.systran.net/translation/text/translate";

  constructor(private http: HttpClient) { }

  translateText(text: string, sourceLang: string, targetLang: string): Observable<string> {
    const url = `${this.apiUrl}?key=${this.apiKey}&source=${sourceLang}&target=${targetLang}&input=${encodeURIComponent(text)}`;
    return this.http.post<any>(url, {}).pipe(
      map(response => response.outputs[0].output)
    );
  }
}
