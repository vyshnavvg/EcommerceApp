import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { MatButton } from '@angular/material/button';

@Component({
  selector: 'app-test-error',
  standalone: true,
  imports: [
    MatButton
  ],
  templateUrl: './test-error.component.html',
  styleUrl: './test-error.component.scss'
})
export class TestErrorComponent {
  baseUrl = "https://localhost:5001/api/";
  private http = inject(HttpClient);
  validationErrors: string[] = [];

  get404Error()
  {
    return this.http.get(this.baseUrl + 'buggy/notfound').subscribe({
      next: resposne => console.log(resposne),
      error: error => console.log(error)
    })
  }

  get400Error()
  {
    return this.http.get(this.baseUrl + 'buggy/badrequest').subscribe({
      next: resposne => console.log(resposne),
      error: error => console.log(error)
    })
  }

  get401Error()
  {
    return this.http.get(this.baseUrl + 'buggy/unauthorized').subscribe({
      next: resposne => console.log(resposne),
      error: error => console.log(error)
    })
  }
  get500Error()
  {
    return this.http.get(this.baseUrl + 'buggy/internalerror').subscribe({
      next: resposne => console.log(resposne),
      error: error => console.log(error)
    })
  }

  get400ValidationError()
  {
    return this.http.post(this.baseUrl + 'buggy/validationerror', {}).subscribe({
      next: resposne => console.log(resposne),
      error: error => this.validationErrors = error
    })
  }

}
