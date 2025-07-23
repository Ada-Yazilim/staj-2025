package com.ada.sigorta.user.controller;

import com.ada.sigorta.customer.model.Customer;
import com.ada.sigorta.customer.repository.CustomerRepository;
import com.ada.sigorta.security.JwtService;
import com.ada.sigorta.user.dto.AuthRequestDto;
import com.ada.sigorta.user.dto.RegisterRequestDto;
import com.ada.sigorta.user.model.Role;
import com.ada.sigorta.user.model.Users;
import com.ada.sigorta.user.repository.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.security.authentication.*;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/auth")
@RequiredArgsConstructor
public class AuthController {

    private final UserRepository userRepository;
    private final CustomerRepository customerRepository;
    private final PasswordEncoder passwordEncoder;
    private final AuthenticationManager authManager;
    private final JwtService jwtService;

    @PostMapping("/register")
    public ResponseEntity<String> register(@RequestBody RegisterRequestDto dto) {
        if (userRepository.existsByUsername(dto.getUsername())) {
            return ResponseEntity.badRequest().body("Kullanıcı adı zaten alınmış.");
        }

        Users user = new Users();
        user.setUsername(dto.getUsername());
        user.setPassword(passwordEncoder.encode(dto.getPassword()));
        user.setRole(dto.getRole() == null ? Role.ROLE_CUSTOMER : dto.getRole());

        userRepository.save(user);

        // Eğer rol CUSTOMER ise customer tablosuna da kaydet
        if (user.getRole() == Role.ROLE_CUSTOMER) {
            Customer customer = new Customer();
            customer.setUser(user);
            customer.setName(dto.getName());
            customer.setPhone(dto.getPhone());
            customer.setAddress(dto.getAddress());
            customer.setNationalId(dto.getNationalId());
            customerRepository.save(customer);
        }

        return ResponseEntity.ok("Kayıt başarılı.");
    }

    @PostMapping("/login")
    public ResponseEntity<String> login(@RequestBody AuthRequestDto authRequest) {
        try {
            authManager.authenticate(
                new UsernamePasswordAuthenticationToken(
                    authRequest.getUsername(),
                    authRequest.getPassword()
                )
            );
        } catch (BadCredentialsException ex) {
            return ResponseEntity.status(401).body("Geçersiz kullanıcı adı veya şifre");
        }

        Users user = userRepository.findByUsername(authRequest.getUsername()).orElseThrow();

        String token = jwtService.generateToken(
            new org.springframework.security.core.userdetails.User(
                user.getUsername(),
                user.getPassword(),
                java.util.List.of(new org.springframework.security.core.authority.SimpleGrantedAuthority(user.getRole().name()))
            )
        );

        return ResponseEntity.ok(token);
    }
}
