export class UserServiceErrorDto {
    message: string;
}


export class UserServiceResponseDto {
    id: number;
    message: string;
    error: UserServiceErrorDto;
    success: boolean;
}