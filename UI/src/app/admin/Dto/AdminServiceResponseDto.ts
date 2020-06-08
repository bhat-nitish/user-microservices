export class UserServiceErrorDto {
    message: string;
}


export class AdminServiceResponseDto {
    id: number;
    message: string;
    error: UserServiceErrorDto;
    success: boolean;
}