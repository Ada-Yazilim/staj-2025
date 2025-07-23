package com.ada.sigorta.user.service;



import com.ada.sigorta.user.model.Role;
import com.ada.sigorta.user.model.Users;
import com.ada.sigorta.user.repository.UserRepository;
import com.ada.sigorta.customer.model.Customer;
import com.ada.sigorta.customer.repository.CustomerRepository;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
public class UserService {

    private final UserRepository userRepository;
    private final CustomerRepository customerRepository;
    private final PasswordEncoder passwordEncoder;

    public UserService(UserRepository userRepository,
                       CustomerRepository customerRepository,
                       PasswordEncoder passwordEncoder) {
        this.userRepository = userRepository;
        this.customerRepository = customerRepository;
        this.passwordEncoder = passwordEncoder;
    }

    @Transactional
    public void registerUser(Users user, Customer customer) {
        
        user.setPassword(passwordEncoder.encode(user.getPassword()));

        
        userRepository.save(user);

        
        if (user.getRole() == Role.ROLE_CUSTOMER) {
            customer.setUser(user);
            customerRepository.save(customer);
        }
    }
}
