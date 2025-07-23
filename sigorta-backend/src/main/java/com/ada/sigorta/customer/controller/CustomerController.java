






package com.ada.sigorta.customer.controller;

import com.ada.sigorta.customer.dto.CustomerRequest;
import com.ada.sigorta.customer.dto.CustomerResponse;
import com.ada.sigorta.customer.service.CustomerService;
import lombok.RequiredArgsConstructor;
import org.springframework.security.core.Authentication;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/customer")
@RequiredArgsConstructor
public class CustomerController {

    private final CustomerService customerService;

    @PostMapping
    public void createOrUpdateCustomer(@RequestBody CustomerRequest request, Authentication authentication) {
        customerService.createOrUpdateCustomer(request, authentication);
    }

    @GetMapping("/me")
    public CustomerResponse getMyCustomerInfo(Authentication authentication) {
        return customerService.getMyCustomerInfo(authentication);
    }
}













/*package com.ada.sigorta.customer.controller;



import com.ada.sigorta.customer.model.Customer;
import com.ada.sigorta.customer.repository.CustomerRepository;
import com.ada.sigorta.user.model.Users;
import com.ada.sigorta.user.repository.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.security.core.Authentication;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/customer")
@RequiredArgsConstructor
public class CustomerController {

	
    private final UserRepository userRepository;
    private final CustomerRepository customerRepository;
    /*
    @GetMapping("/me")
    public Users getMyInfo(Authentication authentication) {
        return userRepository.findByUsername(authentication.getName()).orElseThrow();
    }
    
    
	
	@GetMapping("/me")
	public Customer getMyCustomerInfo(Authentication authentication) {
	    Users user = userRepository.findByUsername(authentication.getName()).orElseThrow();
	    return customerRepository.findByUser(user);
	}

}*/

