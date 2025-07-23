package com.ada.sigorta.user.dto;

import com.ada.sigorta.user.model.Role;
import lombok.Data;

@Data
public class RegisterRequestDto {
    private String username;
    private String password;
    private Role role; 

    
    private String name;
    private String phone;
    private String address;
    private String nationalId;
}
